﻿/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

using OpenNos.Core;
using OpenNos.DAL;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.GameObject
{
    public class Inventory : ThreadSafeSortedList<Guid, ItemInstance>
    {
        #region Members

        private const short DEFAULT_BACKPACK_SIZE = 48;
        private const byte MAX_ITEM_AMOUNT = 99;
        private readonly object _lockObject = new object();

        #endregion

        #region Instantiation

        public Inventory(Character Character)
        {
            Owner = Character;
        }

        #endregion

        #region Properties

        private Character Owner { get; }

        #endregion

        #region Methods

        public static ItemInstance InstantiateItemInstance(short vnum, long ownerId, byte amount = 1)
        {
            ItemInstance newItem = new ItemInstance { ItemVNum = vnum, Amount = amount, CharacterId = ownerId };
            if (newItem.Item != null)
            {
                switch (newItem.Item.Type)
                {
                    case InventoryType.Miniland:
                        newItem.DurabilityPoint = newItem.Item.MinilandObjectPoint / 2;
                        break;

                    case InventoryType.Equipment:
                        newItem = newItem.Item.ItemType == ItemType.Specialist ? new SpecialistInstance
                        {
                            ItemVNum = vnum,
                            SpLevel = 1,
                            Amount = amount
                        } : newItem.Item.ItemType == ItemType.Box ? new BoxInstance
                        {
                            ItemVNum = vnum,
                            Amount = amount
                        } :
                        new WearableInstance
                        {
                            ItemVNum = vnum,
                            Amount = amount
                        };
                        break;
                }
            }

            // set default itemType
            if (newItem.Item != null)
            {
                newItem.Type = newItem.Item.Type;
            }

            return newItem;
        }

        public int BackpackSize()
        {
            return DEFAULT_BACKPACK_SIZE + ((Owner.HaveBackpack() ? 1 : 0) * 12);
        }

        public ItemInstance AddIntoBazaarInventory(InventoryType inventory, byte slot, byte amount)
        {
            ItemInstance inv = LoadBySlotAndType(slot, inventory);
            if (inv == null || amount > inv.Amount)
            {
                return null;
            }
            ItemInstance invcopy = inv.DeepCopy();
            invcopy.Id = Guid.NewGuid();
            if (inv.Item.Type == InventoryType.Equipment)
            {
                for (short i = 0; i < 255; i++)
                {
                    if (LoadBySlotAndType<ItemInstance>(i, InventoryType.Bazaar) == null)
                    {
                        invcopy.Type = InventoryType.Bazaar;
                        invcopy.Slot = i;
                        invcopy.CharacterId = Owner.CharacterId;
                        DeleteFromSlotAndType(inv.Slot, inv.Type);
                        PutItem(invcopy);
                        break;
                    }
                }
                Owner.Session.SendPacket(UserInterfaceHelper.Instance.GenerateInventoryRemove(inventory, slot));
                return invcopy;
            }
            if (amount >= inv.Amount)
            {
                for (short i = 0; i < 255; i++)
                {
                    if (LoadBySlotAndType<ItemInstance>(i, InventoryType.Bazaar) == null)
                    {
                        invcopy.Type = InventoryType.Bazaar;
                        invcopy.Slot = i;
                        invcopy.CharacterId = Owner.CharacterId;
                        DeleteFromSlotAndType(inv.Slot, inv.Type);
                        PutItem(invcopy);
                        break;
                    }
                }
                Owner.Session.SendPacket(UserInterfaceHelper.Instance.GenerateInventoryRemove(inventory, slot));
                return invcopy;
            }

            invcopy.Amount = amount;
            inv.Amount -= amount;

            for (short i = 0; i < 255; i++)
            {
                if (LoadBySlotAndType<ItemInstance>(i, InventoryType.Bazaar) == null)
                {
                    invcopy.Type = InventoryType.Bazaar;
                    invcopy.Slot = i;
                    invcopy.CharacterId = Owner.CharacterId;
                    PutItem(invcopy);
                    break;
                }
            }

            Owner.Session.SendPacket(inv.GenerateInventoryAdd());
            return invcopy;
        }

        public List<ItemInstance> AddNewToInventory(short vnum, byte amount = 1, InventoryType? type = null, sbyte Rare = 0, byte Upgrade = 0, byte Design = 0)
        {
            if (Owner != null)
            {
                ItemInstance newItem = InstantiateItemInstance(vnum, Owner.CharacterId, amount);
                newItem.Rare = Rare;
                newItem.Upgrade = Upgrade;
                newItem.Design = Design;
                return AddToInventory(newItem, type);
            }
            return new List<ItemInstance>();
        }

        public List<ItemInstance> AddToInventory(ItemInstance newItem, InventoryType? type = null)
        {
            List<ItemInstance> invlist = new List<ItemInstance>();
            if (Owner != null)
            {
                ItemInstance inv = null;

                // override type if necessary
                if (type.HasValue)
                {
                    newItem.Type = type.Value;
                }

                if (newItem.Item.Effect == 420 && newItem.Item.EffectValue == 911)
                {
                    newItem.BoundCharacterId = Owner.CharacterId;
                    newItem.DurabilityPoint = (int)newItem.Item.ItemValidTime;
                }

                // check if item can be stapled
                if (newItem.Type != InventoryType.Bazaar && (newItem.Item.Type == InventoryType.Etc || newItem.Item.Type == InventoryType.Main))
                {
                    IEnumerable<ItemInstance> slotNotFull = GetAllItems().Where(i => i.Type != InventoryType.Bazaar && i.Type != InventoryType.PetWarehouse && i.Type != InventoryType.Warehouse && i.Type != InventoryType.FamilyWareHouse && i.ItemVNum.Equals(newItem.ItemVNum) && i.Amount < MAX_ITEM_AMOUNT);
                    int freeslot = BackpackSize() - GetAllItems().Count(s => s.Type == newItem.Type);
                    if (newItem.Amount <= (freeslot * MAX_ITEM_AMOUNT) + slotNotFull.Sum(s => MAX_ITEM_AMOUNT - s.Amount))
                    {
                        foreach (ItemInstance slot in slotNotFull)
                        {
                            int max = slot.Amount + newItem.Amount;
                            max = max > MAX_ITEM_AMOUNT ? MAX_ITEM_AMOUNT : max;
                            newItem.Amount = (byte)(slot.Amount + newItem.Amount - max);
                            newItem.Amount = (byte)(newItem.Amount < 0 ? 0 : newItem.Amount);
                            Logger.LogEvent("ITEM_CREATE", Owner.GenerateIdentity(), $"IIId: {slot.Id} ItemVNum: {slot.ItemVNum} Amount: {max - slot.Amount} MapId: {Owner.MapInstance?.Map.MapId} MapX: {Owner.PositionX} MapY: {Owner.PositionY}");
                            slot.Amount = (byte)max;
                            invlist.Add(slot);
                            Owner.Session?.SendPacket(slot.GenerateInventoryAdd());
                        }
                    }
                }
                if (newItem.Amount > 0)
                {
                    // create new item
                    short? freeSlot = newItem.Type == InventoryType.Wear ? (LoadBySlotAndType((short)newItem.Item.EquipmentSlot, InventoryType.Wear) == null
                                                                         ? (short?)newItem.Item.EquipmentSlot
                                                                         : null)
                                                                         : GetFreeSlot(newItem.Type);
                    if (freeSlot.HasValue)
                    {
                        inv = AddToInventoryWithSlotAndType(newItem, newItem.Type, freeSlot.Value);
                        invlist.Add(inv);
                    }
                }
            }
            return invlist;
        }

        /// <summary>
        /// Add iteminstance to inventory with specified slot and type, iteminstance will be overridden.
        /// </summary>
        /// <param name="itemInstance"></param>
        /// <param name="type"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public ItemInstance AddToInventoryWithSlotAndType(ItemInstance itemInstance, InventoryType type, short slot)
        {
            if (Owner != null)
            {
                Logger.LogEvent("ITEM_CREATE", Owner.GenerateIdentity(), $"IIId: {itemInstance.Id} ItemVNum: {itemInstance.ItemVNum} Amount: {itemInstance.Amount} MapId: {Owner.MapInstance?.Map.MapId} MapX: {Owner.PositionX} MapY: {Owner.PositionY}");

                itemInstance.Slot = slot;
                itemInstance.Type = type;
                itemInstance.CharacterId = Owner.CharacterId;

                if (ContainsKey(itemInstance.Id))
                {
                    Logger.Error(new InvalidOperationException("Cannot add the same ItemInstance twice to inventory."));
                    return null;
                }

                string inventoryPacket = itemInstance.GenerateInventoryAdd();
                if (!string.IsNullOrEmpty(inventoryPacket))
                {
                    Owner.Session?.SendPacket(inventoryPacket);
                }

                if (GetAllItems().Any(s => s.Slot == slot && s.Type == type))
                {
                    return null;
                }
                CheckItemInstanceType(itemInstance);
                this[itemInstance.Id] = itemInstance;
                return itemInstance;
            }
            return null;
        }

        public bool CanAddItem(short itemVnum)
        {
            InventoryType type = ServerManager.Instance.GetItem(itemVnum).Type;
            return CanAddItem(type);
        }

        public int CountItem(int itemVNum)
        {
            return GetAllItems().Where(s => s.ItemVNum == itemVNum && s.Type != InventoryType.FamilyWareHouse && s.Type != InventoryType.Bazaar && s.Type != InventoryType.Warehouse && s.Type != InventoryType.PetWarehouse).Sum(i => i.Amount);
        }

        public int CountItemInAnInventory(InventoryType inv)
        {
            return GetAllItems().Count(s => s.Type == inv);
        }

        public Tuple<short, InventoryType> DeleteById(Guid id)
        {
            if (Owner != null)
            {
                Tuple<short, InventoryType> removedPlace;
                ItemInstance inv = this[id];

                if (inv != null)
                {
                    removedPlace = new Tuple<short, InventoryType>(inv.Slot, inv.Type);
                    Remove(inv.Id);
                }
                else
                {
                    Logger.Error(new InvalidOperationException("Expected item wasn't deleted, Type or Slot did not match!"));
                    return null;
                }

                return removedPlace;
            }
            return null;
        }

        public void DeleteFromSlotAndType(short slot, InventoryType type)
        {
            if (Owner != null)
            {
                ItemInstance inv = GetAllItems().Find(i => i.Slot.Equals(slot) && i.Type.Equals(type));

                if (inv != null)
                {
                    if (Owner.Session.Character.MinilandObjects.Any(s => s.ItemInstanceId == inv.Id))
                    {
                        return;
                    }

                    Remove(inv.Id);
                }
                else
                {
                    Logger.Error(new InvalidOperationException("Expected item wasn't deleted, Type or Slot did not match!"));
                }
            }
        }

        public void DepositItem(InventoryType inventory, byte slot, byte amount, byte NewSlot, ref ItemInstance item, ref ItemInstance itemdest, bool PartnerBackpack)
        {
            if (item != null && amount <= item.Amount && amount > 0)
            {
                MoveItem(inventory, PartnerBackpack ? InventoryType.PetWarehouse : InventoryType.Warehouse, slot, amount, NewSlot, out item, out itemdest);
                Owner.Session.SendPacket(item != null ? item.GenerateInventoryAdd() : UserInterfaceHelper.Instance.GenerateInventoryRemove(inventory, slot));

                if (itemdest != null)
                {
                    Owner.Session.SendPacket(PartnerBackpack ? itemdest.GeneratePStash() : itemdest.GenerateStash());
                }
            }
        }

        public bool EnoughPlace(List<ItemInstance> itemInstances)
        {
            Dictionary<InventoryType, int> place = new Dictionary<InventoryType, int>();
            foreach (IGrouping<short, ItemInstance> itemgroup in itemInstances.GroupBy(s => s.ItemVNum))
            {
                if (itemgroup.FirstOrDefault()?.Type is InventoryType type)
                {
                    List<ItemInstance> listitem = GetAllItems().Where(i => i.Type == type).ToList();
                    if (!place.ContainsKey(type))
                    {
                        place.Add(type, (type != InventoryType.Miniland ? BackpackSize() : 50) - listitem.Count);
                    }

                    int amount = itemgroup.Sum(s => s.Amount);
                    int rest = amount % (type == InventoryType.Equipment ? 1 : 99);
                    bool needanotherslot = listitem.Where(s => s.ItemVNum == itemgroup.Key).Sum(s => MAX_ITEM_AMOUNT - s.Amount) <= rest;
                    place[type] -= (amount / (type == InventoryType.Equipment ? 1 : 99)) + (needanotherslot ? 1 : 0);

                    if (place[type] < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void FDepositItem(InventoryType inventory, byte slot, byte amount, byte newSlot, ref ItemInstance item, ref ItemInstance itemdest)
        {
            if (item != null && amount <= item.Amount && amount > 0 && item.Item.IsTradable && !item.IsBound)
            {
                FamilyCharacter fhead = Owner.Family?.FamilyCharacters.Find(s => s.Authority == FamilyAuthority.Head);
                if (fhead == null)
                {
                    return;
                }
                MoveItem(inventory, InventoryType.FamilyWareHouse, slot, amount, newSlot, out item, out itemdest);
                itemdest.CharacterId = fhead.CharacterId;
                DAOFactory.IteminstanceDAO.InsertOrUpdate(itemdest);
                Owner.Session.SendPacket(item != null ? item.GenerateInventoryAdd() : UserInterfaceHelper.Instance.GenerateInventoryRemove(inventory, slot));
                if (itemdest != null)
                {
                    Owner.Session.SendPacket(itemdest.GenerateFStash());
                    Owner.Family?.InsertFamilyLog(FamilyLogType.WareHouseAdded, Owner.Name, message: $"{itemdest.ItemVNum}|{amount}");
                    DeleteById(itemdest.Id);
                }
            }
        }

        public ItemInstance GetItemInstanceById(Guid id)
        {
            return this[id];
        }

        public T LoadByVNum<T>(short vNum) where T : ItemInstance
        {
            return (T)GetAllItems().Find(i => i.ItemVNum.Equals(vNum));
        }

        public T LoadByItemInstance<T>(Guid id) where T : ItemInstance
        {
            return (T)this[id];
        }

        public T LoadBySlotAndType<T>(short slot, InventoryType type) where T : ItemInstance
        {
            T retItem = null;
            try
            {
                lock (_lockObject)
                {
                    retItem = (T)GetAllItems().SingleOrDefault(i => i?.GetType().Equals(typeof(T)) == true && i.Slot == slot && i.Type == type);
                }
            }
            catch (InvalidOperationException ioEx)
            {
                Logger.LogEventError("LoadBySlotAndType", Owner?.Session?.GenerateIdentity(), "MULTIPLE_ITEMS_IN_SLOT", ioEx);
                bool isFirstItem = true;
                foreach (ItemInstance item in GetAllItems().Where(i => i?.GetType().Equals(typeof(T)) == true && i.Slot == slot && i.Type == type))
                {
                    if (isFirstItem)
                    {
                        retItem = (T)item;
                        isFirstItem = false;
                        continue;
                    }
                    ItemInstance itemInstance = GetAllItems().Find(i => i?.GetType().Equals(typeof(T)) == true && i.Slot == slot && i.Type == type);
                    if (itemInstance != null)
                    {
                        short? freeSlot = GetFreeSlot(type);
                        if (freeSlot.HasValue)
                        {
                            itemInstance.Slot = freeSlot.Value;
                        }
                        else
                        {
                            Remove(itemInstance.Id);
                        }
                    }
                }
            }
            return retItem;
        }

        public ItemInstance LoadBySlotAndType(short slot, InventoryType type)
        {
            ItemInstance retItem = null;
            try
            {
                lock (_lockObject)
                {
                    retItem = GetAllItems().SingleOrDefault(i => i.Slot.Equals(slot) && i.Type.Equals(type));
                }
            }
            catch (InvalidOperationException ioEx)
            {
                Logger.LogEventError("LoadBySlotAndType", Owner?.Session?.GenerateIdentity(), "MULTIPLE_ITEMS_IN_SLOT", ioEx);
                bool isFirstItem = true;
                foreach (ItemInstance item in GetAllItems().Where(i => i.Slot.Equals(slot) && i.Type.Equals(type)))
                {
                    if (isFirstItem)
                    {
                        retItem = item;
                        isFirstItem = false;
                        continue;
                    }
                    ItemInstance itemInstance = GetAllItems().Find(i => i.Slot.Equals(slot) && i.Type.Equals(type));
                    if (itemInstance != null)
                    {
                        short? freeSlot = GetFreeSlot(type);
                        if (freeSlot.HasValue)
                        {
                            itemInstance.Slot = freeSlot.Value;
                        }
                        else
                        {
                            Remove(itemInstance.Id);
                        }
                    }
                }
            }
            return retItem;
        }

        /// <summary>
        /// Moves one item from one Inventory to another. Example: Equipment &lt;-&gt; Wear,
        /// Equipment &lt;-&gt; Costume, Equipment &lt;-&gt; Specialist
        /// </summary>
        /// <param name="sourceSlot"></param>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        /// <param name="targetSlot"></param>
        /// <param name="wear"></param>
        public ItemInstance MoveInInventory(short sourceSlot, InventoryType sourceType, InventoryType targetType, short? targetSlot = null, bool wear = true)
        {
            ItemInstance sourceInstance = LoadBySlotAndType(sourceSlot, sourceType);

            if (sourceInstance == null && wear)
            {
                Logger.Error(new InvalidOperationException("SourceInstance to move does not exist."));
                return null;
            }
            if (Owner != null && sourceInstance != null)
            {
                if (targetSlot.HasValue)
                {
                    if (wear)
                    {
                        // swap
                        ItemInstance targetInstance = LoadBySlotAndType(targetSlot.Value, targetType);

                        sourceInstance.Slot = targetSlot.Value;
                        sourceInstance.Type = targetType;

                        targetInstance.Slot = sourceSlot;
                        targetInstance.Type = sourceType;
                    }
                    else
                    {
                        // move source to target
                        short? freeTargetSlot = GetFreeSlot(targetType);
                        if (freeTargetSlot.HasValue)
                        {
                            sourceInstance.Slot = freeTargetSlot.Value;
                            sourceInstance.Type = targetType;
                        }
                    }

                    return sourceInstance;
                }

                // check for free target slot
                short? nextFreeSlot;
                switch (targetType)
                {
                    case InventoryType.FirstPartnerInventory:
                    case InventoryType.SecondPartnerInventory:
                    case InventoryType.ThirdPartnerInventory:
                    case InventoryType.Wear:
                        nextFreeSlot = (LoadBySlotAndType((short)sourceInstance.Item.EquipmentSlot, targetType) == null
                        ? (short)sourceInstance.Item.EquipmentSlot
                        : (short)-1);
                        break;

                    default:
                        nextFreeSlot = GetFreeSlot(targetType);
                        break;
                }
                if (nextFreeSlot.HasValue)
                {
                    sourceInstance.Type = targetType;
                    sourceInstance.Slot = nextFreeSlot.Value;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            return sourceInstance;
        }

        public void MoveItem(InventoryType sourcetype, InventoryType desttype, short sourceSlot, byte amount, short destinationSlot, out ItemInstance sourceInventory, out ItemInstance destinationInventory)
        {
            Logger.LogEvent("ITEM_MOVE", Owner.GenerateIdentity(), $"SourceType: {sourcetype.ToString()} DestType: {desttype.ToString()} SourceSlot: {sourceSlot} Amount: {amount} DestSlot: {destinationSlot}");

            // load source and destination slots
            sourceInventory = LoadBySlotAndType(sourceSlot, sourcetype);
            destinationInventory = LoadBySlotAndType(destinationSlot, desttype);
            if (sourceInventory != null && amount <= sourceInventory.Amount)
            {
                if (destinationInventory == null)
                {
                    if (sourceInventory.Amount == amount)
                    {
                        sourceInventory.Slot = destinationSlot;
                        sourceInventory.Type = desttype;
                    }
                    else
                    {
                        ItemInstance itemDest = sourceInventory.DeepCopy();
                        sourceInventory.Amount -= amount;
                        itemDest.Amount = amount;
                        itemDest.Type = desttype;
                        itemDest.Id = Guid.NewGuid();
                        AddToInventoryWithSlotAndType(itemDest, desttype, destinationSlot);
                    }
                }
                else
                {
                    if (destinationInventory.ItemVNum == sourceInventory.ItemVNum && (byte)sourceInventory.Item.Type != 0)
                    {
                        if (destinationInventory.Amount + amount > MAX_ITEM_AMOUNT)
                        {
                            int saveItemCount = destinationInventory.Amount;
                            destinationInventory.Amount = MAX_ITEM_AMOUNT;
                            sourceInventory.Amount = (byte)(saveItemCount + sourceInventory.Amount - MAX_ITEM_AMOUNT);
                        }
                        else
                        {
                            destinationInventory.Amount += amount;
                            sourceInventory.Amount -= amount;

                            // item with amount of 0 should be removed
                            if (sourceInventory.Amount == 0)
                            {
                                DeleteFromSlotAndType(sourceInventory.Slot, sourceInventory.Type);
                            }
                        }
                    }
                    else
                    {
                        // add and remove save inventory
                        destinationInventory = TakeItem(destinationInventory.Slot, destinationInventory.Type);
                        if (destinationInventory == null) return;
                        destinationInventory.Slot = sourceSlot;
                        destinationInventory.Type = sourcetype;
                        sourceInventory = TakeItem(sourceInventory.Slot, sourceInventory.Type);
                        if (sourceInventory == null) return;
                        sourceInventory.Slot = destinationSlot;
                        sourceInventory.Type = desttype;
                        PutItem(destinationInventory);
                        PutItem(sourceInventory);
                    }
                }
            }
            sourceInventory = LoadBySlotAndType(sourceSlot, sourcetype);
            destinationInventory = LoadBySlotAndType(destinationSlot, desttype);
        }

        public void RemoveItemAmount(int vnum, int amount = 1)
        {
            if (Owner != null)
            {
                int remainingAmount = amount;

                foreach (ItemInstance inventory in GetAllItems().Where(s => s.ItemVNum == vnum && s.Type != InventoryType.Wear && s.Type != InventoryType.Bazaar && s.Type != InventoryType.Warehouse && s.Type != InventoryType.PetWarehouse && s.Type != InventoryType.FamilyWareHouse).OrderBy(i => i.Slot))
                {
                    if (remainingAmount > 0)
                    {
                        if (inventory.Amount > remainingAmount)
                        {
                            // amount completely removed
                            inventory.Amount -= (byte)remainingAmount;
                            remainingAmount = 0;
                            Owner.Session.SendPacket(inventory.GenerateInventoryAdd());
                        }
                        else
                        {
                            // amount partly removed
                            remainingAmount -= inventory.Amount;
                            DeleteById(inventory.Id);
                            Owner.Session.SendPacket(UserInterfaceHelper.Instance.GenerateInventoryRemove(inventory.Type, inventory.Slot));
                        }
                    }
                    else
                    {
                        // amount to remove reached
                        break;
                    }
                }
            }
        }

        public void RemoveItemFromInventory(Guid id, byte amount = 1)
        {
            if (Owner != null)
            {
                ItemInstance inv = GetAllItems().Find(i => i.Id.Equals(id));
                if (inv != null)
                {
                    inv.Amount -= amount;
                    if (inv.Amount <= 0)
                    {
                        Owner.Session.SendPacket(UserInterfaceHelper.Instance.GenerateInventoryRemove(inv.Type, inv.Slot));
                        Remove(inv.Id);
                        return;
                    }
                    Owner.Session.SendPacket(inv.GenerateInventoryAdd());
                }
            }
        }

        /// <summary>
        /// reorders item in given inventorytype
        /// </summary>
        /// <param name="session"></param>
        /// <param name="inventoryType"></param>
        public void Reorder(ClientSession session, InventoryType inventoryType)
        {
            List<ItemInstance> itemsByInventoryType = new List<ItemInstance>();
            switch (inventoryType)
            {
                case InventoryType.Costume:
                    itemsByInventoryType = GetAllItems().Where(s => s.Type == InventoryType.Costume).OrderBy(s => s.ItemVNum).ToList();
                    break;

                case InventoryType.Specialist:
                    itemsByInventoryType = GetAllItems().Where(s => s.Type == InventoryType.Specialist).OrderBy(s => s.Item.LevelJobMinimum).ToList();
                    break;

                default:
                    itemsByInventoryType = GetAllItems().Where(s => s.Type == inventoryType).OrderBy(s => s.Item.Price).ToList();
                    break;
            }
            GenerateClearInventory(inventoryType);
            for (short i = 0; i < itemsByInventoryType.Count; i++)
            {
                ItemInstance item = itemsByInventoryType[i];
                item.Slot = i;
                this[item.Id].Slot = i;
                session.SendPacket(item.GenerateInventoryAdd());
            }
        }

        private static void CheckItemInstanceType(ItemInstanceDTO itemInstance)
        {
            if (itemInstance != null)
            {
                if (itemInstance.Type == InventoryType.Specialist && !(itemInstance is SpecialistInstance))
                {
                    Logger.Error(new Exception("Cannot add an item of type Specialist without being a SpecialistInstance."));
                    return;
                }
                if ((itemInstance.Type == InventoryType.Equipment || itemInstance.Type == InventoryType.Wear) && !(itemInstance is WearableInstance))
                {
                    Logger.Error(new Exception("Cannot add an item of type Equipment or Wear without being a WearableInstance."));
                }
            }
        }

        private bool CanAddItem(InventoryType type)
        {
            return Owner != null && GetFreeSlot(type).HasValue;
        }

        private void GenerateClearInventory(InventoryType type)
        {
            if (Owner != null)
            {
                for (short i = 0; i < DEFAULT_BACKPACK_SIZE; i++)
                {
                    if (LoadBySlotAndType(i, type) != null)
                    {
                        Owner.Session.SendPacket(UserInterfaceHelper.Instance.GenerateInventoryRemove(type, i));
                    }
                }
            }
        }

        private short? GetFreeSlot(InventoryType type)
        {
            IEnumerable<int> itemInstanceSlotsByType = GetAllItems().Where(i => i.Type == type).OrderBy(i => i.Slot).Select(i => (int)i.Slot);
            IEnumerable<int> instanceSlotsByType = itemInstanceSlotsByType as int[] ?? itemInstanceSlotsByType.ToArray();
            int backpackSize = BackpackSize();
            int nextFreeSlot = instanceSlotsByType.Any() ? Enumerable.Range(0, (type != InventoryType.Miniland ? backpackSize : 50) + 1).Except(instanceSlotsByType).FirstOrDefault() : 0;
            return (short?)nextFreeSlot < (type != InventoryType.Miniland ? backpackSize : 50) ? (short?)nextFreeSlot : null;
        }

        /// <summary>
        /// Puts a Single ItemInstance to the Inventory
        /// </summary>
        /// <param name="itemInstance"></param>
        private void PutItem(ItemInstance itemInstance)
        {
            this[itemInstance.Id] = itemInstance;
        }

        /// <summary>
        /// Takes a Single Inventory including ItemInstance from the List and removes it.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private ItemInstance TakeItem(short slot, InventoryType type)
        {
            ItemInstance itemInstance = GetAllItems().SingleOrDefault(i => i.Slot == slot && i.Type == type);
            if (itemInstance != null)
            {
                Remove(itemInstance.Id);
                return itemInstance;
            }
            return null;
        }

        #endregion
    }
}