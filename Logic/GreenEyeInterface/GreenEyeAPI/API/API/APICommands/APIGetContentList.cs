using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using PlayTubeCore.ContentMangement;

namespace PlayTubeCore.API.APICommands
{
    public class APIGetContentList : IAPICommand
    {
        private string[] mPaths = { "/api/getclips/" };

        public override string[] URLPaths()
        {
            return mPaths;
        }

        protected override string ExecuteDeleteCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            return CreateResponse(pFormat, GetStringParam(pParameters, "CallbackName"), ERR_COMMAND_NOT_SUPPORTED, "Delete command is not supported!", string.Empty, null);
        }

        protected override string ExecuteUpdateCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            return CreateResponse(pFormat, GetStringParam(pParameters, "CallbackName"), ERR_COMMAND_NOT_SUPPORTED, "Update command is not supported!", string.Empty, null);
        }

        protected override string ExecuteGetCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            string StartingLetter = GetStringParam(pParameters, "StartingLetter");

            VideoInfo[] VI;
            if (string.IsNullOrEmpty (StartingLetter) == false)
                VI = ContentManager.Instance().GetContent(StartingLetter);
            else
                VI = ContentManager.Instance().GetAllContent();

            APIResponseClasses.APIRespSearchResult[] SearchResults;
            if (VI != null)
            {
                SearchResults = new APIResponseClasses.APIRespSearchResult[VI.Length];
                for (int i = 0; i < VI.Length; i++)
                {
                    SearchResults[i] = new APIResponseClasses.APIRespSearchResult();
                    SearchResults[i].AuthorName = VI[i].ArtistName;
                    SearchResults[i].ClipName = VI[i].ClipName;
                    SearchResults[i].ImageURL = VI[i].ImageURL;
                    SearchResults[i].PlaybackURL = VI[i].ClipURL;
                    SearchResults[i].VideoID = VI[i].VideoID;
                    SearchResults[i].ViewsCount = VI[i].ViewsCount;
                }
            }
            else
                SearchResults = new APIResponseClasses.APIRespSearchResult[0];

            return CreateResponse(pFormat, 0, "OK", SearchResults);
        }
    }
}
