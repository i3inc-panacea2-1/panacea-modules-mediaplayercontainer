# Panacea.Modules.MediaPlayerContainer

This plugin is responsible to control multiple `IMediaPlayerPlugin` instances and expose programming interfaces to make developing plugins that play media easier.

When developing Panacea plugins, we might need to add support for media. Instead of creating custom video control for each plugin and then searching all loaded plugins to look for `IMediaPlayerPlugin` to play our media, we can simply ask for the IMediaPlayerContainer plugin and let it do the job for us.

## Required plugins
* Panacea.Modularity.UiManager
* Panacea.Modularity.Billing
