using Automatonymous;
using Omnicasa.CloudStorage.Messages;
using System;

namespace Omnicasa.CloudStorage.UploadTrackingService
{
    internal class UploadFileToCloudProcessStateMachine
        : MassTransitStateMachine<UploadFileToCloudProcess>
    {
        public UploadFileToCloudProcessStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => FileSaveToLocal, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => FileUploadToOvh, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => FileUploadToGoogle, x => x.CorrelateById(context => context.Message.CorrelationId));
            CompositeEvent(() => FileUploadCompleted, x => x.UploadStatus, FileUploadToOvh, FileUploadToGoogle);


            Initially(
                    When(FileSaveToLocal)//, context=> context.Data.ServerId == SERVER_ID)
                    .Then(context =>
                    {
                        context.Instance.ServerId = context.Data.ServerId;
                        context.Instance.FileName = context.Data.FileName;
                        context.Instance.CorrelationId = context.Data.CorrelationId;
                        Console.WriteLine($@"I got {context.Data.FileName}");
                    })
                    //.ThenAsync(context => Console.Out.WriteLineAsync("Publish command here"))
                    .TransitionTo(Active)
                );

                //During(Active,//UploadingToCloud
                DuringAny(
                    When(FileUploadToOvh)//, context => context.Data.ServerId == SERVER_ID)
                    .Then(context =>
                    {
                        //Console.WriteLine($@"got FileUploadToOvh event {context.Data.FileName}");
                        //throw new Exception();
                        //Check the both upload to ovh and google success
                        //Then transition to Completed
                        //TransitionToState(context.Instance, UploadingToCloud);
                    }),//.TransitionTo(UploadingToCloud),
                    When(FileUploadToGoogle)//, context => context.Data.ServerId == SERVER_ID)
                    .Then(context =>
                    {
                        //Console.WriteLine($@"got FileUploadToGoogle event {context.Data.FileName}");
                        //Check the both upload to ovh and google success
                        //Then transition to Completed
                    }),//.TransitionTo(UploadingToCloud),
                    When(FileUploadCompleted)//, context => context.Data.ServerId == SERVER_ID)
                    .Then(context =>
                    {
                        Console.WriteLine($@"got FileUploadCompleted event {context.Instance.FileName}");
                                //Check the both upload to ovh and google success
                                //Then transition to Completed
                    }).TransitionTo(Completed)
                    .Finalize()
                );

            SetCompletedWhenFinalized();
        }

        public State Active { get; private set; }
        public State UploadingToCloud { get; private set; }
        public State Completed { get; set; }

        public Event<FileSavedToLocalEvent> FileSaveToLocal { get; private set; }
        public Event<FileUploadedToOvhEvent> FileUploadToOvh { get; private set; }
        public Event<FileUploadedToGoogleEvent> FileUploadToGoogle { get; private set; }
        public Event FileUploadCompleted { get; private set; }
    }
}