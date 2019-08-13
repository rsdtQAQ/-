using Smod2;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Collections.Generic;
using Smod2.API;
using System;

namespace IEventHandler
{
    class EventHandler : IEventHandlerRoundStart, IEventHandlerRoundEnd, IEventHandlerRoundRestart, IEventHandlerSetRole, IEventHandlerPlayerDie, IEventHandlerDoorAccess, IEventHandlerPlayerJoin, IEventHandlerPlayerHurt, IEventHandlerCheckRoundEnd, IEventHandlerUpdate
    {
        private Plugin plugin;
        public string[] door = null;
        public int[] item;
        public static List<string> scp035 = new List<string>();
        private int hp = 500;
        private bool en = true;
        private bool strat = true;
        private int chance = 30;
        private int max = 2;

        public EventHandler(Plugin plugin)
        {
            this.plugin = plugin;
            en = true;
            strat = true;
            scp035 = new List<string>();
            hp = 500;
            chance = 30;
            max = 2;
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            en = plugin.GetConfigBool("scp035_enable");
            scp035 = new List<string>();
            strat = true;
        }

        public void OnRoundEnd(RoundEndEvent ev)
        {
            scp035.Clear();
            scp035 = new List<string>();
            strat = true;
        }

        public void OnRoundRestart(RoundRestartEvent ev)
        {
            scp035.Clear();
            scp035 = new List<string>();
            strat = true;
        }

        public void OnSetRole(PlayerSetRoleEvent ev)
        {
            if (en && strat)
            {
                if (ev.Player.TeamRole.Role == Role.CLASSD || ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY || ev.Player.TeamRole.Role == Role.SCIENTIST || ev.Player.TeamRole.Role == Role.NTF_CADET || ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT || ev.Player.TeamRole.Role == Role.NTF_SCIENTIST || ev.Player.TeamRole.Role == Role.NTF_COMMANDER || ev.Player.TeamRole.Role == Role.FACILITY_GUARD)
                {
                    int n = 0;
                    foreach (Player p in plugin.pluginManager.Server.GetPlayers())
                    {
                        if (scp035.Contains(p.SteamId))
                        {
                            n++;
                        }
                    }
                    max = plugin.GetConfigInt("scp035_number");
                    if (n < max && ev.Player.TeamRole.Role != Role.SPECTATOR)
                    {
                        chance = plugin.GetConfigInt("scp035_spawn");
                        int s = new Random().Next(1, 100);
                        if (s <= chance)
                        {
                            hp = plugin.GetConfigInt("scp035_hp");
                            item = plugin.GetConfigIntList("scp035_item");
                            ev.Player.PersonalClearBroadcasts();
                            ev.Player.PersonalBroadcast(10, "你是<color=red>SCP-035</color>\n偷偷杀掉全部人类\n与<color=red>SCP</color>们合作", false);
                            ev.Player.SetHealth(hp);
                            foreach (int i in item)
                            {
                                ev.Items.Add((ItemType)i);
                            }
                            scp035.Add(ev.Player.SteamId);
                        }
                    }
                }
            }
        }

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            if (scp035.Contains(ev.Player.SteamId))
            {
                scp035.Remove(ev.Player.SteamId);
                plugin.pluginManager.Server.Map.AnnounceScpKill("035", ev.Killer);
            }
        }

        public void OnDoorAccess(PlayerDoorAccessEvent ev)
        {
            if (scp035.Contains(ev.Player.SteamId))
            {
                door = plugin.GetConfigList("scp035_door");
                foreach (string name in door)
                {
                    if (ev.Door.Name == name)
                    {
                        ev.Allow = false;
                    }
                }
            }
        }

        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            if (scp035.Contains(ev.Player.SteamId))
            {
                scp035.Remove(ev.Player.SteamId);
            }
        }

        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            if (scp035.Contains(ev.Player.SteamId) && ev.DamageType == DamageType.POCKET)
            {
                ev.Damage = 0;
                foreach (Player p in plugin.pluginManager.Server.GetPlayers())
                {
                    if (p.TeamRole.Role == Role.SCP_106)
                    {
                        Vector d = p.GetPosition();
                        ev.Player.Teleport(d);
                        break;
                    }
                }
            }
            if (en && ev.DamageType != DamageType.NUKE && ev.DamageType != DamageType.WALL && ev.DamageType != DamageType.DECONT && ev.DamageType != DamageType.LURE && ev.DamageType != DamageType.POCKET && ev.DamageType != DamageType.FLYING && !scp035.Contains(ev.Attacker.SteamId) && !scp035.Contains(ev.Player.SteamId) && !scp035.Contains(ev.Attacker.SteamId) && ev.Attacker is Player && ev.Attacker.SteamId != ev.Player.SteamId)
            {
                if (ev.Player.TeamRole.Team == Smod2.API.Team.CLASSD)
                {
                    if (ev.Attacker.TeamRole.Team == Smod2.API.Team.CLASSD) ev.Damage = 0;
                    if (ev.Attacker.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY) ev.Damage = 0;
                }
                if (ev.Player.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY)
                {
                    if (ev.Attacker.TeamRole.Team == Smod2.API.Team.CLASSD) ev.Damage = 0;
                    if (ev.Attacker.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY) ev.Damage = 0;
                }
                if (ev.Player.TeamRole.Team == Smod2.API.Team.SCIENTIST)
                {
                    if (ev.Attacker.TeamRole.Team == Smod2.API.Team.SCIENTIST) ev.Damage = 0;
                    if (ev.Attacker.TeamRole.Team == Smod2.API.Team.NINETAILFOX) ev.Damage = 0;
                }
                if (ev.Player.TeamRole.Team == Smod2.API.Team.NINETAILFOX)
                {
                    if (ev.Attacker.TeamRole.Team == Smod2.API.Team.SCIENTIST) ev.Damage = 0;
                    if (ev.Attacker.TeamRole.Team == Smod2.API.Team.NINETAILFOX) ev.Damage = 0;
                }
            }
            if (ev.Attacker.TeamRole.Team == Smod2.API.Team.SCP && scp035.Contains(ev.Player.SteamId))
            {
                ev.Damage = 0;
                ev.Attacker.PersonalClearBroadcasts();
                ev.Attacker.PersonalBroadcast(10, "<color=yellow>" + ev.Player.Name + "</color>是<color=red>SCP-035</color>\n请停止攻击", false);
            }
            if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP && scp035.Contains(ev.Attacker.SteamId)) ev.Damage = 0;
            if (scp035.Contains(ev.Player.SteamId) && scp035.Contains(ev.Attacker.SteamId) && ev.Attacker != ev.Player)
            {
                ev.Damage = 0;
                ev.Attacker.PersonalClearBroadcasts();
                ev.Attacker.PersonalBroadcast(10, "<color=yellow>" + ev.Player.Name + "</color>是<color=red>SCP-035</color>\n请停止攻击", false);
            }
        }

        public void OnCheckRoundEnd(CheckRoundEndEvent ev)
        {
            if (en)
            {
                List<string> human = new List<string>();
                List<string> scp = new List<string>();
                List<string> s035 = new List<string>();
                foreach (Player p in plugin.pluginManager.Server.GetPlayers())
                {
                    if (p.TeamRole.Team == Smod2.API.Team.SCP) scp.Add(p.SteamId);
                    if (!scp035.Contains(p.SteamId)) if (p.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY || p.TeamRole.Team == Smod2.API.Team.NINETAILFOX || p.TeamRole.Team == Smod2.API.Team.CLASSD || p.TeamRole.Team == Smod2.API.Team.SCIENTIST) human.Add(p.SteamId);
                    if (scp035.Contains(p.SteamId)) s035.Add(p.SteamId);
                }
                if (scp.Count > 0 && human.Count == 0 && s035.Count > 0) ev.Status = ROUND_END_STATUS.SCP_VICTORY;
                if (s035.Count > 0 && human.Count > 0 && scp.Count == 0) ev.Status = ROUND_END_STATUS.ON_GOING;
                if (scp.Count == 0 && human.Count == 0 && s035.Count > 0) ev.Status = ROUND_END_STATUS.SCP_VICTORY;
                human.Clear();
                scp.Clear();
                s035.Clear();
            }
        }

        public void OnUpdate(UpdateEvent ev)
        {
            if (strat && plugin.pluginManager.Server.Round.Duration > 60)
            {
                strat = false;
            }
        }
    }
}