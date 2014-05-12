using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Web;

namespace CloudServer
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class CloudManager
    {
        [WebGet(UriTemplate = "users/{ClientName}")]
        public string LoginAuth(string ClientName)
        {
            string respFileServerIP = "http://localhost:53003/FileServer";
          //  Stream resp = respFileServerIP;         
            return respFileServerIP;
          //  return "FileServer";
        }

        [WebGet(UriTemplate = "files/GetFiles")]
        public FileItem[] GetFiles()
        {
            using (DataAcess data = new DataAcess())
            {
                //var files = data.GetFiles();
                var files = data.GetFiles();
                List<FileItem> ret = new List<FileItem>();
                foreach (var file in files)
                    //ret.Add(new FileItem() { FileID = file.CloudItemID , Name = file.Name, Description = file.Description, UploadedOn = file.DateTime });
                    ret.Add(new FileItem() { FileID = file.FileID , Name = file.Name, Description = file.Description, UploadedOn = file.DateTime });
                return ret.ToArray();
            }
        }

        [WebInvoke(UriTemplate = "files/UploadFile/{fileName}/{description}", Method = "POST")]
        public void UploadFile(string fileName, string description, Stream fileContents)
        {
            byte[] buffer = new byte[32768];
            MemoryStream ms = new MemoryStream();
            int bytesRead, totalBytesRead = 0;
            do
            {
                bytesRead = fileContents.Read(buffer, 0, buffer.Length);
                totalBytesRead += bytesRead;

                ms.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            // Save the photo on database.
            using (DataAcess data = new DataAcess())
            {
                var file = new StoredFile() { Name = fileName, Description = description, Data = ms.ToArray(), DateTime = DateTime.UtcNow };
//                var photo = new Photo() { Name = fileName, Description = description, Data = ms.ToArray(), DateTime = DateTime.UtcNow };
                data.InsertFile(file);
//                data.InsertPhoto(photo);
            }

            ms.Close();
            Console.WriteLine("Uploaded file {0} with {1} bytes", fileName, totalBytesRead);
        }

        [WebGet(UriTemplate = "files/GetLastFile", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream GetLastFile()
        {
            using (DataAcess data = new DataAcess())
            {
                // Retrieve the last taken photo.
                var file = data.GetLastFile();                
                if (file != null)
                {
                    MemoryStream ms = new MemoryStream(file.Data);
                    return ms;
                }
                else
                {
                    return null;
                }
            }
        }

        [WebGet(UriTemplate = "files/GetFile/{id}", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream GeFile(string id)
        {
            using (DataAcess data = new DataAcess())
            {
                // Retrieve the last taken photo.
                var file = data.GetFile(int.Parse(id));
                if (file != null)
                {
                    MemoryStream ms = new MemoryStream(file.Data);
                    return ms;
                }
                else
                {
                    return null;
                }
            }
        }

        [WebInvoke(UriTemplate = "files/DeleteFile/{id}", Method = "DELETE")]
        public void DeleteFile(string id)
        {
            using (DataAcess data = new DataAcess())
            {
                data.DeleteFile(int.Parse(id));
            }
        }
    }
}