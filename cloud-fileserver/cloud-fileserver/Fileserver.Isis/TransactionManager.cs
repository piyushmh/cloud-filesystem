using System;
using Isis;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace cloudfileserver
{
	public class Transaction
	{
		public string transactionName { get; set; }
		readonly object _locker = new object();
		public bool isTaskFinished = false;
		public bool isTimedOut = false;

		public Transaction (string transName)
		{
			this.transactionName = transName;
		}
		
		public bool waitTillSignalled ()
		{
			lock (_locker) {
				while(!isTaskFinished && !isTimedOut){
					isTimedOut = !Monitor.Wait(_locker,50*1000);
				}
			}
			return !isTimedOut;
		}

		public void signalTransactionEnd ()
		{
			lock (_locker) {
				isTaskFinished = true;
				Monitor.PulseAll (_locker); 
			}
		}
	};
	
	public class TransactionManager{
		public Dictionary<string, Transaction> transaction {get; set;}
		private object privateLock;
		int sequenceNumber;
		
		public TransactionManager ()
		{
			this.transaction = new Dictionary<string, Transaction>();
			this.privateLock = new object();
			this.sequenceNumber = 0;
		}
		
		public Transaction getTransaction (string TransactioName)
		{
			lock (this.privateLock) {
				if (this.transaction.ContainsKey (TransactioName)) {
					return this.transaction [TransactioName];
				}
				return null;
			}
		}
		
		public bool insertTransaction (Transaction trans)
		{
			lock (this.privateLock) {
				if (!this.transaction.ContainsKey (trans.transactionName)) {
					this.transaction.Add (trans.transactionName, trans);
					return true;
				}
				return false;
			}
		}
		
		public Transaction removeAndGetTransaction (string transactionName)
		{
			lock (this.privateLock) {
				Transaction existingTrans = null;
				if (this.transaction.ContainsKey (transactionName)) {
					existingTrans = this.transaction [transactionName];
					this.transaction.Remove(transactionName);
				}
				return existingTrans;
			}
		}
		
		public string generateTransactionId ()
		{
			string transactionID;
			lock (this.privateLock) {
				Address addr = IsisSystem.GetMyAddress ();
				transactionID = addr.ToString();
				transactionID += "_" + this.sequenceNumber;
				transactionID += "_" + DateTime.Now.ToFileTimeUtc();
				this.sequenceNumber++;
			}
			return transactionID;
		}
		
		public string generateTransactionId (string someString)
		{
			string transactionID;
			lock (this.privateLock) {
				Address addr = IsisSystem.GetMyAddress ();
				transactionID = addr.ToString();
				transactionID += "_" + this.sequenceNumber;
				transactionID += "_" + DateTime.Now.ToFileTimeUtc();
				transactionID += "_" + someString;
				this.sequenceNumber++;
			}
			return transactionID;
		}
	}
}

