using Newtonsoft.Json;

namespace PM.Formats.JSON.Services
{
    public class JsonService 
    {
        public static Objects.Read.MarkingTask Read(string fileName)
        {
            string textFromJsonFile = File.ReadAllText(fileName);
            try
            {
                Objects.Read.MarkingTask task = JsonConvert.DeserializeObject<Objects.Read.MarkingTask>(textFromJsonFile);
                return task;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static void Write(Objects.Write.ReportTask reportTask, string fileName)
        {
            string textFromObject = JsonConvert.SerializeObject(reportTask, Formatting.Indented);
            File.WriteAllText(fileName, textFromObject);
        }
        public static bool HasEmtyObjectOrFields(Objects.Read.MarkingTask markingTask)
        {
            if (markingTask == null)
            {
                return true;
            }
            else
            {
                if (markingTask.Nomenclatures == null)
                {
                    return true;
                }
                else
                {
                    foreach (Objects.Read.Nomenclature nomenclature in markingTask.Nomenclatures)
                    {
                        if (string.IsNullOrEmpty(nomenclature.Name) ||
                           string.IsNullOrEmpty(nomenclature.Gtin) ||
                           nomenclature.ArtCode == null ||
                           string.IsNullOrEmpty(nomenclature.Description) ||
                           nomenclature.Attributes == null)
                        {
                            return true;
                        }
                        else
                        {
                            foreach (Objects.Read.Attribute attribute in nomenclature.Attributes)
                            {
                                if (attribute.Code == null ||
                                    string.IsNullOrEmpty(attribute.Value))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
