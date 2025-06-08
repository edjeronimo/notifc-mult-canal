namespace NotificationSystem
{
    public enum NotificationType
    {
        PedidoConfirmado,
        SenhaAlterada,
        PromocaoDisponivel,
    }

    public enum NotificationChannel
    {
        Email,
        SMS,
        PushNotification,
        WhatsApp,
    }

    public enum NotificationPriority
    {
        Alta,
        Media,
        Baixa
    }
}