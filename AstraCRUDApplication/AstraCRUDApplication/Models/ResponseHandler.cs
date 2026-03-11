namespace AstraCRUDApplication.Models
{
    public class ResponseHandler
    {
        private class SuccessResponse
        {
            public string? ResponseCode { get; set; }
            public object? ResponseResult { get; set; }
        }
        private class SuccessResponseWithoutResult
        {
            public required string ResponseCode { get; set; }
        }
        private class FailureResponse
        {
            public string? ResponseCode { get; set; }
            public string? ResponseMessage { get; set; }
        }


        public static object SuccessResponseHelperWithObject(object jsonObject)
        {
            var successResponse = new SuccessResponse
            {
                ResponseCode = "200",
                ResponseResult = jsonObject
            };
            return successResponse;
        }

        public static object SuccessResponseWithoutResultHelper()
        {
            var successResponse = new SuccessResponseWithoutResult
            {
                ResponseCode = "200"
            };
            return successResponse;
        }

        public static object FailureResponseHelper(string failureMessage, string responseCode = "100")
        {
            var failureResponse = new FailureResponse
            {
                ResponseCode = responseCode,
                ResponseMessage = failureMessage
            };
            return failureResponse;
        }
    }
}
