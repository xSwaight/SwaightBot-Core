﻿using Discord;
using Discord.WebSocket;
using Rabbot.Database;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Api.V5;

namespace Rabbot.Services
{
    class TwitchService
    {
        DiscordSocketClient _client;
        private static readonly ILogger _logger = Log.ForContext(Serilog.Core.Constants.SourceContextPropertyName, nameof(TwitchService));
        public TwitchService(DiscordSocketClient client)
        {
            _client = client;
            Task.Run(() =>
            {
                ConfigureLiveMonitorAsync();
            });
        }
        private void ConfigureLiveMonitorAsync()
        {
            try
            {
                var twitchClient = new V5();
                twitchClient.Settings.ClientId = Config.bot.TwitchToken;
                twitchClient.Settings.AccessToken = Config.bot.TwitchAccessToken;

                List<string> usernames = new List<string> { "swaight", "cranbeere" };

                this.OnStreamOnline += Twitch_OnStreamOnline;
                new Task(async () => await CheckStreamStatus(twitchClient, usernames, 60), TaskCreationOptions.LongRunning).Start();
                _logger.Information($"{nameof(TwitchService)}: Loaded successfully");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error while loading {nameof(TwitchService)}");
            }
        }


        private async Task CheckStreamStatus(V5 twitchClient, List<string> usernames, int intervallTime)
        {
            if (!usernames.Any())
                return;

            List<TwitchLib.Api.V5.Models.Streams.Stream> onlineStreams = new List<TwitchLib.Api.V5.Models.Streams.Stream>();
            while (true)
            {
                try
                {
                    await Task.Delay(intervallTime * 1000);
                    foreach (var username in usernames)
                    {
                        var userId = twitchClient.Users.GetUserByNameAsync(username).Result.Matches?.FirstOrDefault()?.Id;
                        if (userId == null)
                            continue;
                        var stream = twitchClient.Streams?.GetStreamByUserAsync(userId).Result?.Stream;
                        if (stream != null)
                        {
                            if (!onlineStreams.Contains(stream))
                            {
                                onlineStreams.Add(stream);
                                OnStreamOnline?.Invoke(this, stream);
                            }
                        }
                        else
                        {
                            if (onlineStreams.Contains(stream))
                            {
                                onlineStreams.Remove(stream);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Error while checking Twitch streams");
                }

            }
        }

        private void Twitch_OnStreamOnline(object sender, TwitchLib.Api.V5.Models.Streams.Stream e)
        {
            try
            {
                Task.Run(async () =>
                {
                    await StreamOnline(e);
                });

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error while pushing event");
            }
        }

        private async Task StreamOnline(TwitchLib.Api.V5.Models.Streams.Stream e)
        {
            try
            {
                using (rabbotContext db = new rabbotContext())
                {
                    var dbStream = db.Stream.FirstOrDefault(p => p.StreamId == Convert.ToInt64(e.Id));
                    if (dbStream != null)
                        return;

                    await db.Stream.AddAsync(new Stream { StreamId = Convert.ToInt64(e.Id), StartTime = e.CreatedAt, Title = e.Channel.Status, TwitchUserId = Convert.ToInt64(e.Channel.Id) });
                    await db.SaveChangesAsync();

                    foreach (var item in db.Guild)
                    {
                        if (item.StreamchannelId != null)
                        {
                            var guild = _client.Guilds.FirstOrDefault(p => p.Id == item.ServerId);
                            if (guild == null)
                                continue;
                            if (!(guild.Channels.FirstOrDefault(p => p.Id == item.StreamchannelId) is SocketTextChannel channel))
                                continue;

                            var embed = new EmbedBuilder();
                            var author = new EmbedAuthorBuilder
                            {
                                Name = e.Channel.DisplayName,
                                IconUrl = e.Channel.Logo
                            };
                            embed.WithAuthor(author);
                            embed.WithTitle(e.Channel.Status);
                            embed.WithUrl($"https://www.twitch.tv/{e.Channel.Name}");
                            embed.WithThumbnailUrl(e.Channel.Logo);
                            embed.AddField("Game", e.Game, true);
                            embed.AddField("Viewers", e.Viewers, true);
                            var ThumbnailUrl = e.Preview.Large.Replace("{width}", "1280").Replace("{height}", "720");
                            embed.WithImageUrl(ThumbnailUrl);
                            if (e.Channel.Name == "swaight")
                                await channel.SendMessageAsync($"Hi {guild.EveryoneRole.Mention}! Ich bin live auf https://www.twitch.tv/{e.Channel.Name} Schaut mal vorbei :)", false, embed.Build());
                            else
                                await channel.SendMessageAsync($"Hi {guild.EveryoneRole.Mention}! {e.Channel.DisplayName} ist live auf https://www.twitch.tv/{e.Channel.Name} Schaut mal vorbei :)", false, embed.Build());

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error while sending stream announcement");
            }
        }

        public event EventHandler<TwitchLib.Api.V5.Models.Streams.Stream> OnStreamOnline;
    }
}
