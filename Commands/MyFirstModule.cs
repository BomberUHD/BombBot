﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace BomBot.Commands
{
	public class MyFirstModule : BaseCommandModule
	{
		public Random Rng { private get; set; }
		[Command("greet")]
		public async Task GreetCommand(CommandContext ctx, DiscordMember member)
		{
			await ctx.RespondAsync($"Greetings, {member.Mention}! Testing CommandsNext");
		}

		[Command("greet")]
		public async Task GreetCommand(CommandContext ctx, DiscordRole role)
		{
			await ctx.TriggerTypingAsync();
			await ctx.RespondAsync($"Greetings, {role.Mention}! Testing CommandsNext");
		}

		[Command("random")]
		public async Task RandomCommand(CommandContext ctx, int min, int max)
		{
			var random = new Random();
			await ctx.RespondAsync($"Your number is: {Rng.Next(min, max)}");
		}
		[Command("remove")]
		[RequirePermissions(Permissions.ManageRoles), RequireUserPermissions(Permissions.ManageRoles)]
		public async Task RemoveRoleCommand(CommandContext ctx, DiscordRole hasthisRole, DiscordRole getsremovedRole)
		{
			string memberlist = "";
			string botlist = "";
			var members = ctx.Guild.GetAllMembersAsync().Result;

			foreach (DiscordMember member in members)
			{
				var allRoles = member.Roles.ToList<DiscordRole>();

				if (allRoles.Contains(hasthisRole) && allRoles.Contains(getsremovedRole))
				{
					string commandissued = string.Format("Remove Role Command issued by {0}#{1}", ctx.Member.Username, ctx.Member.Discriminator);
					await member.RevokeRoleAsync(getsremovedRole, commandissued);

					if (!member.IsBot)
						memberlist += String.Format("\n{0}#{1}", member.Username, member.Discriminator);
					else botlist += String.Format("\n{0}#{1}", member.Username, member.Discriminator);
				}
			}
			string response = string.Format("Removed {0} from these Users:{1}\nand these Bots:{2}", getsremovedRole.Mention, memberlist, botlist);
			await ctx.RespondAsync(response);
		}
		[Command("remove")]
		public async Task RemoveRoleCommand(CommandContext ctx, params string[] names)
		{
			await ctx.RespondAsync("Invalid Syntax: §remove `hasthisRole` `getsremovedRole`");
		}

		[Command("button")]
		public async Task CreateButtonMessage(CommandContext ctx, params string[] names)
		{
            var myButton = new DiscordButtonComponent(ButtonStyle.Primary, "my_very_cool_button", "Very cool button!");

            var builder = new DiscordMessageBuilder()
                .WithContent("This message has buttons! Pretty neat innit?")
                .AddComponents(myButton);

            var message = await builder.SendAsync(ctx.Channel);


			var buttonRes = await message.WaitForButtonAsync();
			Console.WriteLine(message);

            if (!buttonRes.TimedOut)
            {
                await buttonRes.Result.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                await message.ModifyAsync("✅ WaitForButtonAsync() passed");
            }
            else
            {
                await message.ModifyAsync("❎ WaitForButtonAsync() failed");
            }
        }
	}
}
