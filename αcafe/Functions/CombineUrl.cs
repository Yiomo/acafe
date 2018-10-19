using System.Collections.Generic;

namespace αcafe.Functions
{
    class CombineUrl
    {
        public static string CombineRequest(string baseRequest, string apiCase, string model, string lens, List<string> theme, string sortOption, string sortOptionPlus, int Status)
        {
            string baseRequest_temp = baseRequest;
            string apiCase_temp = "";
            string model_temp;
            string lens_temp;
            string theme_temp;
            string sortOption_temp = "";
            string sortOptionPlus_temp;
            string Status_temp;
            string completeRequest;


            switch (apiCase)
            {
                case "file":
                    apiCase_temp = "/file.do?methodName=getFileList&campaignId=16&filterOption=";
                    break;
                case "user":
                    apiCase_temp = "/user.do?methodName=getUserDetails&keys=userId";
                    break;
                case "fileComment":
                    apiCase_temp = "/basfileComment.do?methodName=getUnreadCommentsCount&programId=1";
                    break;
            }


            if (model != null)
            { model_temp = " \"Model\":[\"" + model + "\"], "; }
            else { model_temp = ""; }

            if (lens != null)
            { lens_temp = " \"Lens\":[\"" + lens + "\"], "; }
            else { lens_temp = ""; }

            if (theme != null)
            {
                string theme_temps = "";
                foreach (string item in theme)
                {
                    theme_temps += "\"" + item + "\",";
                }
                theme_temps = theme_temps.TrimEnd(',');
                theme_temp = " \"Theme\":[" + theme_temps + "] ";
            }
            else { theme_temp = ""; }


            switch (sortOption)
            {
                default: sortOption_temp = ""; break;
                case "view":
                    sortOption_temp = "&sortOption=viewD";
                    break;
                case "like":
                    sortOption_temp = "&sortOption=likeD";
                    break;
            }
            if (Status.ToString() != null)
            {
                Status_temp = "&fileStatus=" + Status.ToString();
            }
            else { Status_temp = ""; }
            string[] sortOptionPlus_temps = sortOptionPlus.Split('_');
            string start = sortOptionPlus_temps[0];
            string end = sortOptionPlus_temps[1];
            sortOptionPlus_temp = "&fromNo=" + start + "&toNo=" + end;



            string filterOption;
            filterOption = "{" + model_temp + lens_temp + theme_temp + "}";
            completeRequest = baseRequest_temp + apiCase_temp + filterOption + sortOption_temp + sortOptionPlus_temp + Status_temp;
            return completeRequest;
        }
    }
}
