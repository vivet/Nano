﻿using System;
using System.Linq;
using System.Net;
using Nano.Models.Exceptions;

namespace Nano.Models
{
    /// <summary>
    /// Error.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Message.
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public virtual string[] Exceptions { get; set; } = new string[0];

        /// <summary>
        /// Status Code.
        /// </summary>
        public virtual int StatusCode { get; set; } = 500;

        /// <summary>
        /// Is Translated.
        /// </summary>
        public virtual bool IsTranslated { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Error()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/>.</param>
        public Error(Exception exception)
            : this()
        {
            if (exception == null) 
                throw new ArgumentNullException(nameof(exception));

            this.Summary = "Internal Server Error";
            this.Exceptions = new[] { exception.GetBaseException().Message };
            this.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (exception is AggregateException aggregateException)
            {
                if (aggregateException.InnerException is TranslationException)
                {
                    this.Exceptions = aggregateException.InnerExceptions
                        .Where(x => x is TranslationException)
                        .Select(x => x.Message)
                        .ToArray();

                    this.IsTranslated = true;
                }
            }
            else if (exception is TranslationException)
            {
                this.IsTranslated = true;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var exceptionsString = this.Exceptions
                .Aggregate($"Messages:{Environment.NewLine}", (current, exception) => current + exception + Environment.NewLine);

            return $"{this.StatusCode} {this.Summary}{Environment.NewLine}Messages:{Environment.NewLine}{exceptionsString}";
        }
    }
}