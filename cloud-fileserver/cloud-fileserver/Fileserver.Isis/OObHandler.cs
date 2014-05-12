using Isis;
using System;
using ServiceStack;
using System.IO;
using System.Web;
using System.Collections;
using System.ComponentModel;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using ServiceStack.Configuration;
using ServiceStack.Text;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Serialization.Formatters.Binary;

namespace cloudfileserver
{
	public class OOBHandler
	{
		private static readonly log4net.ILog Logger = 
			log4net.LogManager.GetLogger(typeof(OOBHandler));

		public byte[] ObjectToByteArray(object inputObject)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();    // Create new BinaryFormatter
			MemoryStream memoryStream = new MemoryStream();             // Create target memory stream
			binaryFormatter.Serialize(memoryStream, inputObject);       // Serialize object to stream
			return memoryStream.ToArray();                              // Return stream as byte array
		} 

		public object ByteArrayToObject(byte[] buffer)
		{ 
			BinaryFormatter binaryFormatter = new BinaryFormatter(); // Create new BinaryFormatter
			MemoryStream memoryStream = new MemoryStream(buffer);    // Convert buffer to memorystream
			return binaryFormatter.Deserialize(memoryStream);        // Deserialize stream to an object
		} 

		public MemoryMappedFile createMemoryMappedFile(string mmfFile, int size)
		{
			try
			{
				using (MemoryMappedFile mmf = 
					MemoryMappedFile.CreateFromFile (mmfFile, FileMode.Create, null, size)) {
					return mmf;
				}
			}
			catch (Exception e) {
				Logger.Debug ("Exception caught :" + e);
			}
			return null;
		}

		public MemoryMappedFile openMemoryMappedFile(string mmfFile, int size)
		{
			try
			{
				return MemoryMappedFile.CreateFromFile(mmfFile, FileMode.Open);
			}
			catch (Exception e) {
				Logger.Debug ("Exception caught :" + e);
			}
			return null;
		}

		public MemoryMappedFile serializeIntoMemoryMappedFile(string fileName, object objectData, ref int writtenLength)
		{
			byte[] buffer = ObjectToByteArray(objectData);
			File.WriteAllBytes(fileName,buffer);
			try{
				writtenLength = buffer.Length;
				return MemoryMappedFile.CreateOrOpen(fileName,buffer.Length);
			}
			catch (ArgumentException e) {
				throw e;
			}
			catch (Exception e) {
				throw e;
			}
		}
		
		public object deserializeFromMemoryMappedFile(MemoryMappedFile mmf,ref int index,int length)
		{
			int createLength = (int)length;
			using (MemoryMappedViewAccessor mmfReader = mmf.CreateViewAccessor()) {
				byte[] buffer = new byte[length];
				mmfReader.ReadArray<byte>(index, buffer, 0,createLength);
				return ByteArrayToObject(buffer);
			}
		} 
			
		public int writeObjectToMMF(MemoryMappedFile mmf, int index, object objectData)
		{
			byte[] buffer = ObjectToByteArray(objectData);
			try{
				// Create a view accessor into the file to accommmodate binary data size
				using (MemoryMappedViewAccessor mmfWriter = mmf.CreateViewAccessor(index, buffer.Length))
				{
					// Write the data
					mmfWriter.Write(index,buffer.Length);
					index += buffer.Length;
					mmfWriter.WriteArray<byte>(index, buffer, 0, buffer.Length);
				}
				return index + buffer.Length;
			}
			catch (ArgumentException e) {
				Logger.Debug ("Exception caught :" + e);
				throw e;
			}
			catch (Exception e) {
				Logger.Debug ("Exception caught :" + e);
				throw e;
			}
		}

		public object ReadObjectFromMMF(MemoryMappedFile mmf,ref int index)
		{
			// Get a handle to an existing memory mapped file
			// Create a view accessor from which to read the data
			using (MemoryMappedViewAccessor mmfReader = mmf.CreateViewAccessor())
			{
				// Create a data buffer and read entire MMF view into buffer
				int length = mmfReader.ReadInt32(index);
				index += length;

				byte[] buffer = new byte[length];
				mmfReader.ReadArray<byte>(index, buffer, 0,length);

				// Convert the buffer to a .NET object
				return ByteArrayToObject(buffer);
			}
		} 

		public void sendOOBData (Group group, MemoryMappedFile mmf, string FileName, List<Address> where)
		{
			group.OOBRegister (FileName, mmf);
			group.OOBReReplicate(FileName, where, (Action<string, MemoryMappedFile>) 
				delegate(string oobfname, MemoryMappedFile m) { 
					Logger.Debug ("Send OOB Finished finished for " + FileName);
					//Transfer is Complete Now do a Ordered Send, so that the File may be processed
					//This needs to be ordered, since the all Groups need to see this event in the same wat
					Transaction trn = FileServerComm.getInstance().transManager.getTransaction(oobfname);
					trn.signalTransactionEnd();
				}); 
		}
	}
}
