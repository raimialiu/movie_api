namespace MovieSvc.Application.Common.Model;

 public class ResponseModel 
 {
        public bool Status { get; set; }
        public string Message { get; set; }

        public static ResponseModel Success(string message = null)
        {
            return new ResponseModel()
            {
                Status = true,
                Message = message ?? "Request was Successful"
            };
        }
        public static ResponseModel SuccessWithFailSettlementAccount(string message = null)
        {
            return new ResponseModel()
            {
                Status = true,
                Message = message ?? "Request was Successful with failed settlement account creation"
            };
        }

        public static ResponseModel Failure(string message = null) // , Dictionary<string, string> errors = null
        {
            return new ResponseModel()
            {
                Status = false,
                Message = message ?? "Request was not completed"
            };
        }
    }

    public class ResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }

        public ResponseModel(T data)
        {
            Data = data;
        }

        public ResponseModel()
        {
            
        }

        public static ResponseModel<T> Success(T data, string message = null)
        {
            return new ResponseModel<T>()
            {
                Status = true,
                Message = message ?? "Request was Successful",
                Data = data
            };
        }

        public static ResponseModel<T> Failure(T data, string message = null)
        {
            return new ResponseModel<T>()
            {
                Status = false,
                Message = message ?? "Request was not completed",
                Data = data
            };
        }

        public new static ResponseModel<T> Failure(string message = null) //added dev1
        {
            return new ResponseModel<T>()
            {
                Status = false,
                Message = message ?? "Request was not completed",
                Data = default
            };
        }
}

public class ResponseErrorModel : ResponseModel
{
    public IDictionary<string, string[]> Errors { get; set; }

    public List<string> ErrorsList { get; set; }

    public static ResponseModel Failure(List<string> errors = null, string message = null)
    {
        return new ResponseErrorModel()
        {
            Message = message ?? "Request was not completed",
            ErrorsList = errors ?? new List<string>(),
            Errors = default
        };
    }

    public static ResponseModel Failure(IDictionary<string, string[]> errors = null, string message = null)
    {
        return new ResponseErrorModel()
        {
            Message = message ?? "Request was not completed",
            Errors = errors ?? new Dictionary<string, string[]>(),
            ErrorsList = default
        };
    }
    
}