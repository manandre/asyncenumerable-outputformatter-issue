using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IO.Swagger.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace IO.Swagger.Formatters
{
    /// <summary>
    /// 
    /// </summary>
    public class TsvOutputFormatter : TextOutputFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        public TsvOutputFormatter()
        {
            // Add the supported media type.
            SupportedMediaTypes.Add("text/tsv");
            SupportedEncodings.Add(Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override bool CanWriteType(System.Type type)
        {
            var expected = type == typeof(IAsyncEnumerable<Pet>);
            Debug.Assert(expected); // Only this type is used as output format in this project
            return type == typeof(IAsyncEnumerable<Pet>);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="selectedEncoding"></param>
        /// <returns></returns>
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var type = context.ObjectType;
            if (type != typeof(IAsyncEnumerable<Pet>))
            {
                throw new NotSupportedException("Received type is not supported: " + type);
            }

            var value = context.Object as IAsyncEnumerable<Pet>;
            await using (var writer = new StreamWriter(context.HttpContext.Response.Body))
            {
                await writer.WriteLineAsync("id");
                await foreach (var item in value.ConfigureAwait(false))
                {
                    await writer.WriteLineAsync(item.Id.ToString());
                }
            }
        }
    }
}