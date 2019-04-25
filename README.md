# Panacea.Modules.MediaPlayerContainer

This plugin is responsible to control multiple `IMediaPlayerPlugin` instances and expose programming interfaces to make developing plugins that play media easier.

When developing Panacea plugins, we might need to add support for media. Instead of creating custom video control for each plugin and then searching all loaded plugins to look for `IMediaPlayerPlugin` to play our media, we can simply ask for the IMediaPlayerContainer plugin and let it do the job for us.

It contains UI to play, pause, stop, fullscreen media. Supports multiple IMediaPlayerPlugins that can open a specific type (asks user to pick and allows them to change later). Supports Picture in Picture.

Finally, it integrates with Billing.

## Required plugins
* Panacea.Modularity.UiManager
* Panacea.Modularity.Billing
