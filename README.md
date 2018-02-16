# SIMPLSharp-PlexWebhooks

Crestron SIMPL# Pro library for consuming PLEX webhook events.

## Getting Started

[Configure](https://support.plex.tv/articles/115002267687-webhooks/) your PLEX media server to send webhooks to your control system.

Create a new instance of **WebHookListener** with the port you specified when you configured the webhook.

Subscribe to the PlexMediaEvent.

Do Magic.



See the [JsonResponse.txt](JsonResponse.txt) for an example payload that the webhook will deliver.

### Prerequisites

1. Crestron 3-Series Control System
2. PLEX Media Server
3. A Keyboard

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
