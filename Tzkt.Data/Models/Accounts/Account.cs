﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tzkt.Data.Models
{
    public abstract class Account
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public AccountType Type { get; set; }
        public int FirstLevel { get; set; }
        public int LastLevel { get; set; }

        public long Balance { get; set; }
        public int Counter { get; set; }

        public int DelegationsCount { get; set; }
        public int OriginationsCount { get; set; }
        public int TransactionsCount { get; set; }
        public int RevealsCount { get; set; }

        public int? DelegateId { get; set; }
        public int? DelegationLevel { get; set; }
        public bool Staked { get; set; }

        #region relations
        [ForeignKey(nameof(DelegateId))]
        public Delegate Delegate { get; set; }

        [ForeignKey(nameof(FirstLevel))]
        public Block FirstBlock { get; set; }
        #endregion

        public override string ToString() => Address;
    }

    public static class AccountModel
    {
        public static void BuildAccountModel(this ModelBuilder modelBuilder)
        {
            #region indexes
            modelBuilder.Entity<Account>()
                .HasIndex(x => x.Id)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(x => x.Address)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(x => x.DelegateId);
            #endregion

            #region keys
            modelBuilder.Entity<Account>()
                .HasKey(x => x.Id);
            #endregion

            #region props
            modelBuilder.Entity<Account>()
                .HasDiscriminator<AccountType>(nameof(Account.Type))
                .HasValue<User>(AccountType.User)
                .HasValue<Delegate>(AccountType.Delegate)
                .HasValue<Contract>(AccountType.Contract);

            modelBuilder.Entity<Account>()
                .Property(x => x.Address)
                .IsFixedLength(true)
                .HasMaxLength(36)
                .IsRequired();
            #endregion

            #region relations
            modelBuilder.Entity<Account>()
                .HasOne(x => x.FirstBlock)
                .WithMany(x => x.CreatedAccounts)
                .HasForeignKey(x => x.FirstLevel)
                .HasPrincipalKey(x => x.Level);

            modelBuilder.Entity<Account>()
                .HasOne(x => x.Delegate)
                .WithMany(x => x.DelegatedAccounts)
                .HasForeignKey(x => x.DelegateId);
            #endregion
        }
    }

    public enum AccountType : byte
    {
        User,
        Delegate,
        Contract
    }
}
