﻿using GalacticWasteManagement.Output;
using JellyDust;
using System;
using System.Data.SqlClient;

namespace GalacticWasteManagement
{
    internal class TransactionManager : IDisposable
    {
        private readonly SqlConnectionStringBuilder sqlConnectionStringBuilder;
        private readonly IOutput output;
        private readonly bool singleTransaction;
        IUnitOfWork _uow = null;

        public TransactionManager(SqlConnectionStringBuilder sqlConnectionStringBuilder, IOutput output, bool singleTransaction = true)
        {
            this.sqlConnectionStringBuilder = sqlConnectionStringBuilder;
            this.output = output;
            this.singleTransaction = singleTransaction;
        }
        public ITransaction Transaction
        {
            get
            {
                if (_uow == null)
                {
                    _uow = new UnitOfWork(new TransactionFactory(), new ConnectionFactory(sqlConnectionStringBuilder.ConnectionString, output));
                }
                if (!singleTransaction)
                {
                    if (_uow != null)
                    {
                        Dispose();
                    }
                    _uow = new UnitOfWork(new TransactionFactory(), new ConnectionFactory(sqlConnectionStringBuilder.ConnectionString, output));
                }

                return _uow.Transaction;
            }
        }

        public IConnection Connection
        {
            get
            {
                if (_uow == null)
                {
                    _uow = new UnitOfWork(new TransactionFactory(), new ConnectionFactory(sqlConnectionStringBuilder.ConnectionString, output));
                }

                return _uow.Connection;
            }
        }

        public void Dispose()
        {
            try
            {
                _uow?.Commit();
            }
            finally
            {
                _uow?.Dispose();
                _uow = null;
            }
            
        }
    }
}