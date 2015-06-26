using Newtonsoft.Json;
using Orders_Core.Ports.Commands;
using paramore.brighter.commandprocessor;

namespace Orders_Service.Ports.Mappers
{
    class CompleteOrderCommandMessageMapper : IAmAMessageMapper<CompleteOrderCommand>
    {
        public Message MapToMessage(CompleteOrderCommand request)
        {
            var header = new MessageHeader(messageId: request.Id, topic: "Order.Complete", messageType: MessageType.MT_EVENT);
            var body = new MessageBody(JsonConvert.SerializeObject(request));
            var message = new Message(header, body);
            return message;
        }

        public CompleteOrderCommand MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<CompleteOrderCommand>(message.Body.Value);
        }
    }
}
