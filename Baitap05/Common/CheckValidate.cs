namespace Common
{
    public class CheckValidate
    {
        public static bool CheckXSSInput(string input)
        {
            try
            {
                var listdangerousString = new List<string> { "<applet", "<body", "<embed", "<frame", "<script", "<frameset", "<html", "<iframe", "<img", "<style", "<layer", "<link", "<ilayer", "<meta", "<object", "<h", "<input", "<a", "&lt", "&gt" };
                if (string.IsNullOrEmpty(input)) return false;
                foreach (var dangerous in listdangerousString)
                {
                    if (input.Trim().ToLower().IndexOf(dangerous) >= 0) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool CheckValidateInput(string input)
        {
            var result = true;
            // Check xem dữ liệu đầu vào có bị trống không
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            //kiểm tra xem có ký tự đặc biệt hoặc dữ liệu nguy hiểm không
            if (!CheckXSSInput(input))
            {
                return false;
            }
            // kiểm tra xem có phải kiểu số không
            int n;
            bool isNumeric = int.TryParse(input, out n);

            if (!isNumeric)
            {
                return false;
            }
            return result;

        }
    }
}
