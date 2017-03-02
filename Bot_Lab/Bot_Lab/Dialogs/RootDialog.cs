using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Bot_Lab.Dialogs
{
    [Serializable]
    [LuisModel("3a581190-2dc8-4066-a7be-f9caea6fb287", "9eba2173d4034b079ef08af9a85c1fd1")]
    //[LuisModel("1a8ad562-4d60-464b-9400-df49f6919f0e", "d195de5fa46443d5b0b207520495cb6e")]
    //[LuisModel("d195de5fa46443d5b0b207520495cb6e", "628d0fe3-08aa-4c7b-ab94-1eba6dc15516")]
    public class RootDialog : LuisDialog<object>
    {
        
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
        [LuisIntent("SayHello")]
        public async Task getUserInfo(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Hi, I am GM's IT Buddy.  I can help you get IT services.  Lets find out who to submit the requests for. You will need to provide a few details.");
                // var userID = new FormDialog<UserIDDialog>(new UserIDDialog(), UserIDDialog.BuildUserIDForm, FormOptions.PromptInStart);
                // context.Call(userID, userIDComplete);
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
                context.Wait(MessageReceived);
            }
        }
        
       /* public async Task SayHello(IDialogContext context, LuisResult result)
        {
            context.Call(new HelloDialog(), this.ResumeAfterUserHelloDialog);
        }*/
        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("type a statement like 'I want to install...'  or 'I am new to a project' and send in the chat");
            context.Wait(MessageReceived);
        }
        private async Task userIDComplete(IDialogContext context, IAwaitable<UserIDDialog> result)
        {
            try
            {
                var request = await result;
                await context.PostAsync("Thanks for the using GM's IT Buddy.");
                //use a card for showing their data
                var resultMessage = context.MakeMessage();
                //resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();
                string ThankYouMessage;
                if (request.RequestForOptions == UserIDDialog.RequestFor.Self)
                {
                    ThankYouMessage = request.Name + ", thank you for joining us for dinner, we look forward to having you and your guests.";
                }
                else
                {
                    ThankYouMessage = request.Name + ", thank you for joining us for dinner, we look forward to having you and your guests for the " + request.RequestForOptions;
                }
                ThumbnailCard thumbnailCard = new ThumbnailCard()
                {
                    Title = String.Format("User {0}", request.GMID),
                    Subtitle = String.Format("{1} email: {0}", request.Name, request.Email),
                    Text = ThankYouMessage,
                    Images = new List<CardImage>()
                        {
                        new CardImage() { Url = "https://upload.wikimedia.org/wikipedia/en/e/ee/Unknown-person.gif" }
                        },
                };
                resultMessage.Attachments.Add(thumbnailCard.ToAttachment());
                await context.PostAsync(resultMessage);
                await context.PostAsync(String.Format(""));
            }
            catch (FormCanceledException)
            {
                await context.PostAsync("You canceled the transaction, ok. ");
            }
            catch (Exception ex)
            {
                var exDetail = ex;
                await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
            }
            finally
            {
                context.Wait(this.MessageReceived);
            }
        }

      /*  private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                //capture which option then selected
                string optionSelected = await result;
                switch (optionSelected)
                {
                    case ReservartionOption:
                        break;
                    case HelloOption:
                        context.Call(new HelloDialog(), this.ResumeAfterUserHelloDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                var exDetail = ex;
                //If too many attempts we send error to user and start all over.
                await context.PostAsync($"Ooops! Too many attempts :( You can start again!");
                //This sets us in a waiting state, after running the prompt again.
                context.Wait(this.MessageReceivedAsync);
            }
        }*/

        private async Task ResumeAfterUserHelloDialog(IDialogContext context,  IAwaitable<object> result)
        { }
        /*private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }*/

    }
 }