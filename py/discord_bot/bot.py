import discord
from discord.ext import commands

import config

PREFIX = '.'
COMMANDS = ['hello', 'clear', 'clear_contain', 'clear_commands']
HELLO_WORDS = ['hello', 'hey', 'hi']

client = commands.Bot(command_prefix=PREFIX)

@client.event
async def on_ready():
	print('Bot_connected')

#command_hello
@client.command(pass_context=True)
async def hello(ctx, arg='Hello!'):
	await ctx.channel.purge(limit = 1)
	author = ctx.message.author
	await ctx.send(f'{author.mention} {arg}')

#answer on message
# @client.event
# async def on_message(message):
# 	msg = message.content.lower()
# 	if msg in HELLO_WORDS:
# 		await message.channel.send('Хелоу, чё надо?')

#command_clear
@client.command(pass_context=True)
async def clear(ctx, amount: int=100):
	await ctx.channel.purge(limit = amount)

#command_clear
@client.command(pass_context=True)
async def clear_contain(ctx, base: str = None, amount: int = 100):
	def contain(message):
		return base.lower() in message.content.lower()
	await ctx.channel.purge(limit = amount, check=contain)

#command_clear_commands
@client.command(pass_context=True)
async def clear_commands(ctx, amount: int=100):
	def isCommand(message):
		content = message.content
		if content.startswith('.'):
			content = content[1:]
		return content.split(' ', 1)[0] in COMMANDS
	await ctx.channel.purge(limit = amount, check=isCommand)

# connect
client.run(config.TOKEN)
