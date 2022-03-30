using MediatR;
using System;
using System.Collections.Generic;

namespace Boyner.CaseStudy.ApplicationCore.Commands
{
    public class SavePostCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public SavePostCommand(string id, string text)
        {
            Id = id;
            Text = text;
        }

    }

}