using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Client.Formatters
{
	public class RawRequestBodyInputFormatter : InputFormatter
    {
        public override Boolean CanRead(InputFormatterContext context)
        {
            return (context.HttpContext.Request.ContentType == "application/json") ? false : true;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			var request = context.HttpContext.Request;
			using (var reader = new StreamReader(request.Body))
			{
				try
				{
					var content = await reader.ReadToEndAsync();
					return await InputFormatterResult.SuccessAsync(content);
				}
				catch
				{
					return await InputFormatterResult.FailureAsync();
				}
			}
		}
    }
}