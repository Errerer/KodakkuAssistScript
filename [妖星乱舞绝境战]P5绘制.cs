using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using KodakkuAssist.Module.Draw;
using KodakkuAssist.Module.GameEvent;
using KodakkuAssist.Module.GameOperate;
using KodakkuAssist.Script;
using Newtonsoft.Json;

namespace ErrerScriptNamespace
{
    [ScriptType(
        name: "[妖星乱舞绝境战]P5全流程绘制",
        territorys: [1363],
        guid: "b3f7c1a2-8d4e-4f6a-9c12-5e8a1b3d7f90",
        version: "0.0.13",
        author: "Errer",
        note: "P5全套：地火步进圈 + 钢铁月环 + 地水/洪水2穿1安全点 + 核爆/神圣分散。\n" +
              "更新三星踩塔指路、地火后分散绘制。")]
    public class YW_P5FireScript
    {
        #region Settings

        [UserSetting("-----地火步进圈-----")]
        public bool _____Fire_Settings_____ { get; set; } = true;

        [UserSetting("启用步进圈绘制")]
        public bool EnableFireDraw { get; set; } = true;

        [UserSetting("地火AOE圆圈半径")]
        public float FireCircleRadius { get; set; } = 6f;

        [UserSetting("步进距离（每个圈间距）")]
        public float FireStepDistance { get; set; } = 7f;

        [UserSetting("步进次数（画几个圈）")]
        public int FireStepCount { get; set; } = 6;

        [UserSetting("提前显示（读条结束前多少ms开始画）")]
        public int AdvanceDrawMs { get; set; } = 1500;

        [UserSetting("地火圈显示时长（ms）")]
        public int DisplayDurationMs { get; set; } = 3500;

        [UserSetting("-----钢铁/月环（二选一的灾祟）-----")]
        public bool _____SteelDonut_Settings_____ { get; set; } = true;

        [UserSetting("启用钢铁月环绘制")]
        public bool EnableSteelDonut { get; set; } = true;

        [UserSetting("钢铁范围半径")]
        public float SteelRadius { get; set; } = 10f;

        [UserSetting("月环安全圈半径")]
        public float DonutInnerRadius { get; set; } = 10f;

        [UserSetting("月环危险圈外径")]
        public float DonutOuterRadius { get; set; } = 50f;

        [UserSetting("钢铁/月环显示时长（ms）")]
        public int SteelDonutDurationMs { get; set; } = 8000;

        [UserSetting("-----地水（矩形AOE）-----")]
        public bool _____Water_Settings_____ { get; set; } = true;

        [UserSetting("启用地水绘制")]
        public bool EnableWater { get; set; } = true;

        [UserSetting("地水延迟（ms）")]
        public int WaterDelayMs { get; set; } = 4000;

        [UserSetting("地水显示时长（ms）")]
        public int WaterDurationMs { get; set; } = 1800;

        [UserSetting("地水矩形宽度")]
        public float WaterWidth { get; set; } = 10f;

        [UserSetting("地水矩形长度")]
        public float WaterLength { get; set; } = 60f;

        [UserSetting("地水后偏移")]
        public float WaterBackOffset { get; set; } = 30f;

        [UserSetting("地水颜色")]
        public ScriptColor WaterColor { get; set; } = new ScriptColor { V4 = new Vector4(1.0f, 1.0f, 0.0f, 0.35f) };

        [UserSetting("-----洪水2穿1安全点-----")]
        public bool _____FloodSafe_Settings_____ { get; set; } = true;

        [UserSetting("启用洪水2穿1安全点")]
        public bool EnableFloodSafe { get; set; } = true;

        [UserSetting("洪水安全点距场中距离（米）")]
        public float FloodSafeDistance { get; set; } = 13.5f;

        [UserSetting("洪水安全点最大距离（米）")]
        public float FloodMaxDist { get; set; } = 8f;

        [UserSetting("洪水2交点延迟（ms）")]
        public int Flood2DelayMs { get; set; } = 0;

        [UserSetting("洪水2交点持续（ms）")]
        public int Flood2DurationMs { get; set; } = 3500;

        [UserSetting("洪水1交点延迟（ms）")]
        public int Flood1DelayMs { get; set; } = 3500;

        [UserSetting("洪水1交点持续（ms）")]
        public int Flood1DurationMs { get; set; } = 3000;

        [UserSetting("Debug：打印洪水交点坐标")]
        public bool FloodDebug { get; set; } = false;

        [UserSetting("-----核爆/分散（癫狂交响曲）-----")]
        public bool _____BusterSpread_Settings_____ { get; set; } = true;

        [UserSetting("启用核爆分散绘制")]
        public bool EnableBusterSpread { get; set; } = true;

        [UserSetting("核爆（双T死刑）范围半径")]
        public float BusterRadius { get; set; } = 25f;

        [UserSetting("核爆显示时长（ms）")]
        public int BusterDurationMs { get; set; } = 4000;

        [UserSetting("神圣（随机分散）范围半径")]
        public float SpreadRadius { get; set; } = 5f;

        [UserSetting("神圣显示时长（ms）")]
        public int SpreadDurationMs { get; set; } = 5000;

        [UserSetting("-----咏唱中分散圈-----")]
        public bool _____CastSpread_Settings_____ { get; set; } = true;

        [UserSetting("启用咏唱中分散圈")]
        public bool EnableCastSpread { get; set; } = true;

        [UserSetting("咏唱中分散圈半径")]
        public float CastSpreadRadius { get; set; } = 5f;

        [UserSetting("咏唱中分散圈颜色")]
        public ScriptColor CastSpreadColor { get; set; } = new ScriptColor { V4 = new Vector4(1.0f, 1.0f, 0.0f, 0.35f) };

        [UserSetting("-----三星踩塔指路-----")]
        public bool _____Tower_Settings_____ { get; set; } = true;

        [UserSetting("启用三星踩塔指路")]
        public bool EnableTowerGuide { get; set; } = true;

        [UserSetting("踩塔只指路自己")]
        public bool TowerGuideSelfOnly { get; set; } = true;

        [UserSetting("踩塔头部标记")]
        public bool TowerEnableMark { get; set; } = false;

        [UserSetting("踩塔Debug")]
        public bool TowerDebug { get; set; } = false;

        [UserSetting("踩塔线色")]
        public ScriptColor TowerGuideColor { get; set; } = new ScriptColor { V4 = new Vector4(0f, 1f, 1f, 1f) };

        [UserSetting("-----颜色-----")]
        public bool _____Color_Settings_____ { get; set; } = true;

        [UserSetting("地火危险色")]
        public ScriptColor FireDangerColor { get; set; } = new ScriptColor { V4 = new Vector4(1.0f, 0.3f, 0.0f, 0.35f) };

        [UserSetting("钢铁危险色")]
        public ScriptColor SteelColor { get; set; } = new ScriptColor { V4 = new Vector4(1.0f, 0.0f, 0.0f, 0.35f) };

        [UserSetting("月环安全色（内圈）")]
        public ScriptColor DonutSafeColor { get; set; } = new ScriptColor { V4 = new Vector4(0.0f, 1.0f, 0.0f, 0.25f) };

        [UserSetting("月环危险色（外圈）")]
        public ScriptColor DonutDangerColor { get; set; } = new ScriptColor { V4 = new Vector4(1.0f, 0.0f, 0.0f, 0.35f) };

        [UserSetting("核爆危险色")]
        public ScriptColor BusterColor { get; set; } = new ScriptColor { V4 = new Vector4(1.0f, 0.0f, 0.0f, 0.35f) };

        [UserSetting("神圣黄色")]
        public ScriptColor SpreadColor { get; set; } = new ScriptColor { V4 = new Vector4(1.0f, 1.0f, 0.0f, 0.35f) };

        #endregion

        #region State

        private const string DrawPrefix = "Errer_YW_P5Fire";
        private const string TowerPrefix = "Errer_P5T";
        private static readonly Vector3 ArenaCenter = new(100f, 0f, 100f);
        private static readonly Vector3 TowerAnchorA = new(100f, 0f, 85f);

        private const DrawModeEnum DM = DrawModeEnum.Default;
        private const DrawModeEnum DMG = DrawModeEnum.Imgui;
        private const int TowerGuideDurationMs = 6500;

        private int _fireCount;

        private enum TowerElement { Fire = 2015294, Ice = 2015295, Thunder = 2015296 }

        private static readonly MarkType[] TowerMarks =
        {
            MarkType.Attack1, MarkType.Attack2, MarkType.Attack3, MarkType.Attack4,
            MarkType.Stop1, MarkType.Stop2, MarkType.Bind1, MarkType.Bind2
        };

        private static readonly string[] TowerLabels = { "MT", "ST", "H1", "H2", "D1", "D2", "D3", "D4" };

        private readonly object _towerLock = new();
        private readonly Dictionary<Vector3, TowerElement> _towerRoundTowers = new();
        private readonly Dictionary<int, (TowerElement Element, DateTime ExpireAt)> _towerDebuffs = new();
        private readonly Dictionary<int, Vector3> _towerRawTargets = new();
        private readonly Dictionary<int, int> _towerMarkOrder = new();
        private readonly HashSet<TowerElement> _towerSeenDouble = new();
        private readonly HashSet<int> _towerIdlePlayers = new();
        private bool _towerCooling, _towerIdleLocked;
        private int _towerMechanicSeq;
        private int _towerRound;

        // ── 洪水2穿1：每4个读条为一组，异向配对 ──
        private int _floodCount;
        private int _floodGroupCount;
        private readonly Vector3[] _floodNegPos = new Vector3[2]; private readonly float[] _floodNegRot = new float[2]; private int _negCount;
        private readonly Vector3[] _floodPosPos = new Vector3[2]; private readonly float[] _floodPosRot = new float[2]; private int _posCount;
        private Vector3? _savedFloodSafe1;

        #endregion

        #region Init

        public void Init(ScriptAccessory accessory)
        {
            _fireCount = 0;
            _floodCount = 0;
            _floodGroupCount = 0;
            _negCount = 0; _posCount = 0;
            _savedFloodSafe1 = null;
            ResetTowerGuideState(true);
            accessory.Method.RemoveDraw($"{DrawPrefix}_.*");
            accessory.Method.RemoveDraw($"{TowerPrefix}_.*");
        }

        #endregion

        #region 地火步进圈

        [ScriptMethod(name: "地火步进圈", eventType: EventTypeEnum.StartCasting,
            eventCondition: ["ActionId:47932"], userControl: false)]
        public void OnFireCast(Event @event, ScriptAccessory accessory)
        {
            _fireCount++;
            var round = _fireCount;
            // var isEven = round % 2 == 0;
            // var pairIndex = (round + 1) / 2;

            var pos = JsonConvert.DeserializeObject<Vector3>(@event["SourcePosition"]);
            var rot = float.Parse(@event["SourceRotation"]);
            var castDurationMs = DurationMs(@event, 5000);
            var delayMs = Math.Max(0, castDurationMs - AdvanceDrawMs);
            var destroyAtMs = delayMs + DisplayDurationMs;

            if (EnableFireDraw)
            {
                var dir = RotationToDirection(rot);
                var fireColor = FireDangerColor.V4;
                for (var i = 0; i < FireStepCount; i++)
                {
                    var circlePos = pos + dir * (FireStepDistance * i);
                    var dp = accessory.Data.GetDefaultDrawProperties();
                    dp.Name = $"{DrawPrefix}_Fire{round}_Step{i}";
                    dp.Position = circlePos;
                    dp.Scale = new Vector2(FireCircleRadius);
                    dp.Color = fireColor;
                    dp.Delay = delayMs;
                    dp.DestoryAt = destroyAtMs;
                    dp.ScaleMode = ScaleMode.None;
                    accessory.Method.SendDraw(DM, DrawTypeEnum.Circle, dp);
                }
            }

            // ── 安全点（暂注释）──
            /*
            if (isEven && _hasPrev)
            {
                var d1 = RotationToDirection(_prevRot);
                var d2 = RotationToDirection(rot);
                var pt = LineIntersection2D(_prevPos, d1, pos, d2);

                Vector3 safePos;
                if (pt != null)
                    safePos = ClampToDistance(pt.Value, ArenaCenter, SafeDistance);
                else
                    safePos = FallbackSafePos(_prevPos, pos);

                var safeDelay = delayMs;
                var safeDestroy = safeDelay + SafeDisplayDurationMs;

                var dpSafe = accessory.Data.GetDefaultDrawProperties();
                dpSafe.Name = $"{DrawPrefix}_Safe{pairIndex}";
                dpSafe.Position = safePos;
                dpSafe.Scale = new Vector2(SafeCircleRadius);
                dpSafe.Color = SafePointColor.V4;
                dpSafe.Delay = safeDelay;
                dpSafe.DestoryAt = safeDestroy;
                dpSafe.ScaleMode = ScaleMode.None;
                accessory.Method.SendDraw(DM, DrawTypeEnum.Circle, dpSafe);

                var dpGuide = accessory.Data.GetDefaultDrawProperties();
                dpGuide.Name = $"{DrawPrefix}_Guide{pairIndex}";
                dpGuide.Owner = accessory.Data.Me;
                dpGuide.TargetPosition = safePos;
                dpGuide.Scale = new Vector2(1.5f);
                dpGuide.ScaleMode = ScaleMode.YByDistance;
                dpGuide.Color = GuideColor.V4;
                dpGuide.Delay = safeDelay;
                dpGuide.DestoryAt = safeDestroy;
                accessory.Method.SendDraw(DMG, DrawTypeEnum.Displacement, dpGuide);

                _hasPrev = false;
            }
            else if (!isEven)
            {
                _prevPos = pos;
                _prevRot = rot;
                _hasPrev = true;
            }
            */
        }

        #endregion

        #region 钢铁/月环（二选一的灾祟）

        [ScriptMethod(name: "钢铁（地震）", eventType: EventTypeEnum.StartCasting,
            eventCondition: ["ActionId:49742"], userControl: false)]
        public void OnSteel(Event @event, ScriptAccessory accessory)
        {
            if (!EnableSteelDonut) return;
            if (!TryParseObjectId(@event["SourceId"], out var sid)) return;

            var dp = accessory.Data.GetDefaultDrawProperties();
            dp.Name = $"{DrawPrefix}_Steel";
            dp.Owner = sid;
            dp.Scale = new Vector2(SteelRadius);
            dp.Color = SteelColor.V4;
            dp.DestoryAt = SteelDonutDurationMs;
            dp.ScaleMode = ScaleMode.None;
            accessory.Method.SendDraw(DM, DrawTypeEnum.Circle, dp);
        }

        [ScriptMethod(name: "月环（龙卷）安全+危险", eventType: EventTypeEnum.StartCasting,
            eventCondition: ["ActionId:49743"], userControl: false)]
        public void OnDonut(Event @event, ScriptAccessory accessory)
        {
            if (!EnableSteelDonut) return;
            if (!TryParseObjectId(@event["SourceId"], out var sid)) return;

            // 内圈安全区（绿色空心）
            var dpSafe = accessory.Data.GetDefaultDrawProperties();
            dpSafe.Name = $"{DrawPrefix}_DonutSafe";
            dpSafe.Owner = sid;
            dpSafe.Scale = new Vector2(DonutInnerRadius);
            dpSafe.Color = DonutSafeColor.V4;
            dpSafe.DestoryAt = SteelDonutDurationMs;
            dpSafe.ScaleMode = ScaleMode.None;
            accessory.Method.SendDraw(DM, DrawTypeEnum.Circle, dpSafe);

            // 外圈危险区（红色月环）
            var dpDanger = accessory.Data.GetDefaultDrawProperties();
            dpDanger.Name = $"{DrawPrefix}_DonutDanger";
            dpDanger.Owner = sid;
            dpDanger.Scale = new Vector2(DonutOuterRadius);
            dpDanger.InnerScale = new Vector2(DonutInnerRadius);
            dpDanger.Radian = MathF.PI * 2f;
            dpDanger.Color = DonutDangerColor.V4;
            dpDanger.DestoryAt = SteelDonutDurationMs;
            dpDanger.ScaleMode = ScaleMode.None;
            accessory.Method.SendDraw(DM, DrawTypeEnum.Donut, dpDanger);
        }

        #endregion

        #region 咏唱中分散圈

        [ScriptMethod(name: "咏唱中分散圈", eventType: EventTypeEnum.StartCasting,
            eventCondition: ["ActionId:47934"], userControl: false)]
        public void OnCastSpread(Event @event, ScriptAccessory accessory)
        {
            if (!EnableCastSpread) return;

            var durationMs = DurationMs(@event, 4700) + 1000;
            for (var i = 0; i < Math.Min(8, accessory.Data.PartyList.Count); i++)
            {
                var dp = accessory.Data.GetDefaultDrawProperties();
                dp.Name = $"{DrawPrefix}_CastSpread{i}";
                dp.Owner = accessory.Data.PartyList[i];
                dp.Scale = new Vector2(CastSpreadRadius);
                dp.Color = CastSpreadColor.V4;
                dp.DestoryAt = durationMs;
                dp.ScaleMode = ScaleMode.None;
                accessory.Method.SendDraw(DM, DrawTypeEnum.Circle, dp);
            }
        }

        #endregion

        #region 三星踩塔指路

        [ScriptMethod(name: "三星踩塔机制开始", eventType: EventTypeEnum.StartCasting,
            eventCondition: ["ActionId:47938"], userControl: false)]
        public void OnTowerMechanicStart(Event @event, ScriptAccessory accessory)
        {
            if (!EnableTowerGuide) return;

            int seq;
            lock (_towerLock)
            {
                ClearTowerGuideState(false);
                _towerCooling = false;
                _towerIdleLocked = false;
                seq = ++_towerMechanicSeq;
                _towerRound = 0;
            }

            accessory.Method.RemoveDraw($"{TowerPrefix}_.*");
            var delay = DurationMs(@event, 4700) + 1000;
            if (TowerDebug) accessory.Method.SendChat($"/e [塔] 机制开始，{delay}ms 后锁定闲人");
            _ = Task.Run(async () => { await Task.Delay(delay); LockTowerIdlePlayers(accessory, seq); });
        }

        [ScriptMethod(name: "三星踩塔塔收集", eventType: EventTypeEnum.ObjectEffect,
            eventCondition: ["Id1:16"], userControl: false)]
        public void OnTowerObjectEffect(Event @event, ScriptAccessory accessory)
        {
            if (!EnableTowerGuide) return;

            var pos = JsonConvert.DeserializeObject<Vector3>(@event["SourcePosition"]);
            if (!TryParseObjectId(@event["SourceId"], out var sourceId)) return;
            var obj = accessory.Data.Objects.SearchById(sourceId);
            if (obj == null) return;
            var dataId = obj.DataId;
            if (dataId is not (2015294 or 2015295 or 2015296)) return;

            bool shouldAssign;
            int count;
            lock (_towerLock)
            {
                _towerRoundTowers.TryAdd(RoundTowerPos(pos), (TowerElement)dataId);
                count = _towerRoundTowers.Count;
                shouldAssign = count >= 4 && !_towerCooling;
                if (shouldAssign) _towerCooling = true;
            }

            if (TowerDebug) accessory.Method.SendChat($"/e [塔] {DateTime.Now:HH:mm:ss.fff} {TowerElementLabel((TowerElement)dataId)} ({pos.X:F1},{pos.Z:F1}) n={count}");
            if (shouldAssign) _ = Task.Run(async () => { await Task.Delay(800); AssignTowerGuides(accessory); });
        }

        [ScriptMethod(name: "三星踩塔易伤", eventType: EventTypeEnum.StatusAdd,
            eventCondition: ["StatusID:regex:^(2902|2903|2998)$"], userControl: false)]
        public void OnTowerDebuff(Event @event, ScriptAccessory accessory)
        {
            if (!EnableTowerGuide) return;
            if (!TryParseObjectId(@event["TargetId"], out var targetId)) return;

            var partyIndex = accessory.Data.PartyList.IndexOf(targetId);
            if (partyIndex < 0) return;

            var element = int.Parse(@event["StatusID"]) switch
            {
                2902 => TowerElement.Fire,
                2903 => TowerElement.Ice,
                2998 => TowerElement.Thunder,
                _ => TowerElement.Fire
            };
            var expireAt = DateTime.Now.AddMilliseconds(DurationMs(@event, 20000));
            lock (_towerLock) _towerDebuffs[partyIndex] = (element, expireAt);
            if (TowerDebug) accessory.Method.SendChat($"/e [易伤] {TowerPlayerLabel(partyIndex)} → {TowerElementLabel(element)} until {expireAt:HH:mm:ss.fff}");
        }

        private void AssignTowerGuides(ScriptAccessory accessory)
        {
            List<(Vector3 Pos, TowerElement Element, float Angle)> towers;
            Dictionary<int, (TowerElement Element, DateTime ExpireAt)> debuffs;
            int round;
            lock (_towerLock)
            {
                towers = _towerRoundTowers.Select(kv => (kv.Key, kv.Value, TowerAngle(kv.Key))).OrderBy(t => t.Item3).ToList();
                _towerRoundTowers.Clear();
                debuffs = _towerDebuffs.ToDictionary(kv => kv.Key, kv => kv.Value);
                round = ++_towerRound;
                _towerRawTargets.Clear();
                _towerMarkOrder.Clear();
                _towerCooling = false;
            }

            if (towers.Count != 4)
            {
                if (TowerDebug) accessory.Method.SendChat($"/e [塔] 第{round}轮塔数异常 {towers.Count}，跳过");
                return;
            }

            var groups = towers.GroupBy(t => t.Element).ToDictionary(g => g.Key, g => g.ToList());
            if (groups.Count != 3 || groups.Values.Count(g => g.Count == 2) != 1)
            {
                if (TowerDebug) accessory.Method.SendChat($"/e [塔] 第{round}轮元素分布异常 {string.Join(" ", groups.Select(g => $"{TowerElementLabel(g.Key)}x{g.Value.Count}"))}");
                return;
            }

            var doubleElement = groups.First(g => g.Value.Count == 2).Key;
            if (!_towerSeenDouble.Add(doubleElement) && TowerDebug) accessory.Method.SendChat($"/e [塔] 双塔元素重复：{TowerElementLabel(doubleElement)}");
            if (TowerDebug)
            {
                accessory.Method.SendChat($"/e ═══ 第{round}轮 塔{towers.Count} 双{TowerElementLabel(doubleElement)} ═══");
                foreach (var t in towers) accessory.Method.SendChat($"/e   {TowerElementLabel(t.Element)}塔 @({t.Pos.X:F1},{t.Pos.Z:F1}) ang={t.Angle:F2}");
            }

            var used = new HashSet<int>();
            var markIndex = 0;
            void Put(int partyIndex, Vector3 pos, string label)
            {
                if (markIndex >= TowerMarks.Length || partyIndex < 0 || partyIndex >= accessory.Data.PartyList.Count || !used.Add(partyIndex)) return;
                lock (_towerLock)
                {
                    _towerRawTargets[partyIndex] = pos;
                    _towerMarkOrder[partyIndex] = markIndex;
                }
                DrawTowerGuide(accessory, partyIndex, pos);
                if (TowerDebug) accessory.Method.SendChat($"/e   分配 {TowerPlayerLabel(partyIndex)} debuff:{(debuffs.TryGetValue(partyIndex, out var d) ? TowerElementLabel(d.Element) : "无")} → {label}");
                markIndex++;
            }

            var idlePlayers = GetTowerIdlePlayers();
            var activeDebuffs = debuffs
                .Where(kv => !idlePlayers.Contains(kv.Key) && kv.Value.ExpireAt > DateTime.Now.AddMilliseconds(3000))
                .OrderBy(kv => kv.Key)
                .ToList();
            foreach (var kv in activeDebuffs)
            {
                var target = ClockwiseFirstDifferentTower(towers, kv.Value.Element);
                if (target.HasValue) Put(kv.Key, target.Value.Pos, $"顺异:{TowerElementLabel(target.Value.Element)}");
            }

            var idleTarget = CounterClockwiseFromTowerAnchor(towers, doubleElement);
            foreach (var partyIndex in idlePlayers)
            {
                if (markIndex >= TowerMarks.Length) break;
                if (!used.Contains(partyIndex)) Put(partyIndex, idleTarget.Pos, $"逆双:{TowerElementLabel(doubleElement)}");
            }

            if (TowerDebug) TowerSummary(accessory, towers);
        }

        private void DrawTowerGuide(ScriptAccessory accessory, int partyIndex, Vector3 pos)
        {
            if (partyIndex >= accessory.Data.PartyList.Count) return;
            var owner = accessory.Data.PartyList[partyIndex];
            if (TowerGuideSelfOnly && owner != accessory.Data.Me) return;

            int markIndex;
            lock (_towerLock) markIndex = _towerMarkOrder.GetValueOrDefault(partyIndex, 0);
            if (TowerEnableMark) accessory.Method.Mark(owner, TowerMarks[Math.Clamp(markIndex, 0, TowerMarks.Length - 1)]);

            var dp = accessory.Data.GetDefaultDrawProperties();
            dp.Name = $"{TowerPrefix}_g{partyIndex}";
            dp.Owner = owner;
            dp.TargetPosition = pos;
            dp.Scale = new Vector2(1.5f);
            dp.ScaleMode = ScaleMode.YByDistance;
            dp.Color = TowerGuideColor.V4;
            dp.DestoryAt = TowerGuideDurationMs;
            accessory.Method.SendDraw(DMG, DrawTypeEnum.Displacement, dp);
        }

        private void LockTowerIdlePlayers(ScriptAccessory accessory, int seq)
        {
            List<int> idle;
            int debuffCount;
            lock (_towerLock)
            {
                if (seq != _towerMechanicSeq || _towerIdleLocked) return;
                var now = DateTime.Now.AddMilliseconds(3000);
                var active = _towerDebuffs.Where(kv => kv.Value.ExpireAt > now).Select(kv => kv.Key).ToHashSet();
                idle = Enumerable.Range(0, Math.Min(8, accessory.Data.PartyList.Count)).Where(i => !active.Contains(i)).OrderBy(i => i).ToList();
                debuffCount = active.Count;
                _towerIdlePlayers.Clear();
                foreach (var i in idle) _towerIdlePlayers.Add(i);
                _towerIdleLocked = true;
            }

            if (TowerDebug) accessory.Method.SendChat($"/e [塔] 锁定闲人：{string.Join(" ", idle.Select(TowerPlayerLabel))} debuffs={debuffCount} idle={idle.Count}");
        }

        private List<int> GetTowerIdlePlayers()
        {
            lock (_towerLock)
            {
                return _towerIdlePlayers.OrderBy(i => i).ToList();
            }
        }

        private void ResetTowerGuideState(bool advanceSeq)
        {
            lock (_towerLock)
            {
                ClearTowerGuideState(advanceSeq);
                _towerCooling = false;
                _towerIdleLocked = false;
                _towerRound = 0;
            }
        }

        private void ClearTowerGuideState(bool advanceSeq)
        {
            _towerRoundTowers.Clear();
            _towerDebuffs.Clear();
            _towerRawTargets.Clear();
            _towerMarkOrder.Clear();
            _towerSeenDouble.Clear();
            _towerIdlePlayers.Clear();
            if (advanceSeq) _towerMechanicSeq++;
        }

        private void TowerSummary(ScriptAccessory accessory, List<(Vector3 Pos, TowerElement Element, float Angle)> towers)
        {
            Dictionary<int, Vector3> raw;
            lock (_towerLock) raw = new Dictionary<int, Vector3>(_towerRawTargets);
            accessory.Method.SendChat("/e ── 踩塔汇总 ──");
            foreach (var t in towers)
            {
                var who = string.Join(" ", Enumerable.Range(0, 8).Where(i => raw.ContainsKey(i) && RoundTowerPos(raw[i]) == RoundTowerPos(t.Pos)).Select(TowerPlayerLabel));
                accessory.Method.SendChat($"/e   {TowerElementLabel(t.Element)}塔 @({t.Pos.X:F1},{t.Pos.Z:F1}) → {who}");
            }
        }

        private static (Vector3 Pos, TowerElement Element, float Angle)? ClockwiseFirstDifferentTower(List<(Vector3 Pos, TowerElement Element, float Angle)> towers, TowerElement element)
        {
            var anchors = towers.Where(t => t.Element == element).OrderBy(t => t.Angle).ToList();
            if (anchors.Count == 2 && ClockwiseTowerDelta(anchors[1].Angle, anchors[0].Angle) < ClockwiseTowerDelta(anchors[0].Angle, anchors[1].Angle))
                anchors.Reverse();
            foreach (var anchor in anchors)
            {
                var target = towers.Where(t => t.Element != element).OrderBy(t => ClockwiseTowerDelta(anchor.Angle, t.Angle)).FirstOrDefault();
                if (target != default) return target;
            }
            return null;
        }

        private static (Vector3 Pos, TowerElement Element, float Angle) CounterClockwiseFromTowerAnchor(List<(Vector3 Pos, TowerElement Element, float Angle)> towers, TowerElement element)
        {
            var anchorAngle = TowerAngle(TowerAnchorA);
            return towers.Where(t => t.Element == element).OrderBy(t => CounterClockwiseTowerDelta(anchorAngle, t.Angle)).First();
        }

        private static float ClockwiseTowerDelta(float from, float to) => (to - from + MathF.PI * 2f) % (MathF.PI * 2f);

        private static float CounterClockwiseTowerDelta(float from, float to) => (from - to + MathF.PI * 2f) % (MathF.PI * 2f);

        private static Vector3 RoundTowerPos(Vector3 pos) => new(MathF.Round(pos.X, 1), 0f, MathF.Round(pos.Z, 1));

        private static float TowerAngle(Vector3 pos)
        {
            var angle = MathF.Atan2(pos.X - ArenaCenter.X, ArenaCenter.Z - pos.Z);
            return angle < 0f ? angle + MathF.PI * 2f : angle;
        }

        private static string TowerElementLabel(TowerElement element) => element switch
        {
            TowerElement.Fire => "火",
            TowerElement.Ice => "冰",
            TowerElement.Thunder => "雷",
            _ => "?"
        };

        private static string TowerPlayerLabel(int index) => index >= 0 && index < TowerLabels.Length ? TowerLabels[index] : $"P{index}";

        #endregion

        #region 核爆/分散（癫狂交响曲）

        [ScriptMethod(name: "核爆（双T死刑）", eventType: EventTypeEnum.StatusAdd,
            eventCondition: ["StatusID:5350"], userControl: false)]
        public void OnBuster(Event @event, ScriptAccessory accessory)
        {
            if (!EnableBusterSpread) return;
            if (!TryParseObjectId(@event["TargetId"], out var tid)) return;

            var dp = accessory.Data.GetDefaultDrawProperties();
            dp.Name = $"{DrawPrefix}_Buster";
            dp.Owner = tid;
            dp.Scale = new Vector2(BusterRadius);
            dp.Color = BusterColor.V4;
            dp.DestoryAt = BusterDurationMs;
            dp.ScaleMode = ScaleMode.None;
            accessory.Method.SendDraw(DM, DrawTypeEnum.Circle, dp);
        }

        [ScriptMethod(name: "神圣（随机分散）", eventType: EventTypeEnum.StatusAdd,
            eventCondition: ["StatusID:5351"], userControl: false)]
        public void OnSpread(Event @event, ScriptAccessory accessory)
        {
            if (!EnableBusterSpread) return;
            if (!TryParseObjectId(@event["TargetId"], out var tid)) return;

            var dp = accessory.Data.GetDefaultDrawProperties();
            dp.Name = $"{DrawPrefix}_Spread";
            dp.Owner = tid;
            dp.Scale = new Vector2(SpreadRadius);
            dp.Color = SpreadColor.V4;
            dp.DestoryAt = SpreadDurationMs;
            dp.ScaleMode = ScaleMode.None;
            accessory.Method.SendDraw(DM, DrawTypeEnum.Circle, dp);
        }

        #endregion

        #region 地水（矩形AOE）

        [ScriptMethod(name: "地水", eventType: EventTypeEnum.StartCasting,
            eventCondition: ["ActionId:49539"], userControl: false)]
        public void OnWater(Event @event, ScriptAccessory accessory)
        {
            if (!EnableWater) return;

            var pos = JsonConvert.DeserializeObject<Vector3>(@event["SourcePosition"]);
            var rot = float.Parse(@event["SourceRotation"]);

            _floodCount++;

            if (FloodDebug)
                accessory.Method.SendChat($"/e [洪水] #{_floodCount} 读条 pos=({pos.X:F1},{pos.Z:F1}) rot={rot:F3}");

            // 矩形中心 = Boss位置 - 前方*偏移（矩形向后方延伸）
            var dir = new Vector3(MathF.Sin(rot), 0f, MathF.Cos(rot));
            var center = pos - dir * WaterBackOffset;

            var dp = accessory.Data.GetDefaultDrawProperties();
            dp.Name = $"{DrawPrefix}_Water";
            dp.Position = center;
            dp.Rotation = rot;
            dp.Scale = new Vector2(WaterWidth, WaterLength);
            dp.Color = WaterColor.V4;
            dp.Delay = WaterDelayMs;
            dp.DestoryAt = WaterDurationMs;
            dp.ScaleMode = ScaleMode.None;
            accessory.Method.SendDraw(DM, DrawTypeEnum.Rect, dp);

            // ── 洪水2穿1：每4个读条异向配对 ──
            if (EnableFloodSafe)
            {
                if (rot < 0f && _negCount < 2) { _floodNegPos[_negCount] = pos; _floodNegRot[_negCount] = rot; _negCount++; }
                else if (rot >= 0f && _posCount < 2) { _floodPosPos[_posCount] = pos; _floodPosRot[_posCount] = rot; _posCount++; }

                if (_negCount >= 1 && _posCount >= 1 && _negCount + _posCount >= 4)
                {
                    _floodGroupCount++;

                    // 遍历所有 neg×pos 组合，取距场中最近的交点
                    Vector3? best = null; float bestDist = float.MaxValue;
                    for (int ni = 0; ni < _negCount; ni++)
                    for (int pi = 0; pi < _posCount; pi++)
                    {
                        var dn = new Vector3(MathF.Sin(_floodNegRot[ni]), 0f, MathF.Cos(_floodNegRot[ni]));
                        var dpo = new Vector3(MathF.Sin(_floodPosRot[pi]), 0f, MathF.Cos(_floodPosRot[pi]));
                        var pt = LineIntersection2D(_floodNegPos[ni], dn, _floodPosPos[pi], dpo);
                        if (pt == null) continue;
                        var d = Vector3.Distance(pt.Value, ArenaCenter);
                        if (d < bestDist) { best = pt.Value; bestDist = d; }
                    }
                    _negCount = 0; _posCount = 0;

                    if (best == null || bestDist > FloodMaxDist)
                    {
                        if (FloodDebug)
                            accessory.Method.SendChat($"/e [洪水] 组{_floodGroupCount} 交点距场中={bestDist:F1}m 超出阈值{FloodMaxDist:F1}m ✗");
                        return;
                    }
                    var safePos = best.Value;

                    // 向场中偏移4米
                    var toCenter = ArenaCenter - safePos;
                    var toCenterLen = toCenter.Length();
                    if (toCenterLen > 0.001f)
                        safePos += toCenter / toCenterLen * 4f;

                    if (FloodDebug)
                        accessory.Method.SendChat($"/e [洪水] 组{_floodGroupCount} 交点=({safePos.X:F1},{safePos.Z:F1}) 距场中={bestDist:F1}m ✓");

                    if (_floodGroupCount == 1)
                        _savedFloodSafe1 = safePos;
                    else if (_floodGroupCount == 2)
                    {
                        DrawFloodSafe(accessory, safePos, 2, Flood2DelayMs, Flood2DurationMs);
                        if (_savedFloodSafe1.HasValue)
                            DrawFloodSafe(accessory, _savedFloodSafe1.Value, 1, Flood1DelayMs, Flood1DurationMs);
                    }
                }
            }
        }

        private void DrawFloodSafe(ScriptAccessory a, Vector3 pos, int idx, int delayMs, int durationMs)
        {
            if (FloodDebug)
                a.Method.SendChat($"/e [洪水] 绘制安全点{idx} pos=({pos.X:F1},{pos.Z:F1}) 延迟={delayMs}ms 持续={durationMs}ms");

            var dpSafe = a.Data.GetDefaultDrawProperties();
            dpSafe.Name = $"{DrawPrefix}_FloodSafe{idx}";
            dpSafe.Position = pos;
            dpSafe.Scale = new Vector2(1.5f);
            dpSafe.Color = new Vector4(0f, 1f, 0f, 1f);
            dpSafe.Delay = delayMs;
            dpSafe.DestoryAt = durationMs;
            dpSafe.ScaleMode = ScaleMode.None;
            a.Method.SendDraw(DM, DrawTypeEnum.Circle, dpSafe);

            var dpGuide = a.Data.GetDefaultDrawProperties();
            dpGuide.Name = $"{DrawPrefix}_FloodGuide{idx}";
            dpGuide.Owner = a.Data.Me;
            dpGuide.TargetPosition = pos;
            dpGuide.Scale = new Vector2(1.5f);
            dpGuide.ScaleMode = ScaleMode.YByDistance;
            dpGuide.Color = new Vector4(0f, 1f, 1f, 1f);
            dpGuide.Delay = delayMs;
            dpGuide.DestoryAt = durationMs;
            a.Method.SendDraw(DMG, DrawTypeEnum.Displacement, dpGuide);
        }

        #endregion

        #region Helpers

        private static Vector3 RotationToDirection(float radians)
        {
            return new Vector3(MathF.Sin(radians), 0f, MathF.Cos(radians));
        }

        private static bool TryParseObjectId(string str, out uint id)
        {
            id = 0;
            if (string.IsNullOrEmpty(str)) return false;
            str = str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? str[2..] : str;
            return uint.TryParse(str, System.Globalization.NumberStyles.HexNumber,
                System.Globalization.CultureInfo.InvariantCulture, out id);
        }

        private static Vector3? LineIntersection2D(Vector3 p1, Vector3 d1, Vector3 p2, Vector3 d2)
        {
            var crossD = d1.X * d2.Z - d1.Z * d2.X;
            if (MathF.Abs(crossD) < 0.0001f) return null;
            var delta = p2 - p1;
            var t = (delta.X * d2.Z - delta.Z * d2.X) / crossD;
            return new Vector3(p1.X + t * d1.X, 0f, p1.Z + t * d1.Z);
        }

        private static Vector3 ClampToDistance(Vector3 p, Vector3 c, float maxD)
        {
            var d = p - c;
            var len = d.Length();
            if (len < 0.001f || len <= maxD) return p;
            return c + d / len * maxD;
        }

        private Vector3 FloodFallback(Vector3 p1, Vector3 p2)
        {
            var mid = (p1 + p2) * 0.5f;
            var dir = mid - ArenaCenter;
            if (dir.Length() < 0.001f) dir = new Vector3(1f, 0f, 0f);
            else dir = Vector3.Normalize(dir);
            return ArenaCenter + dir * FloodSafeDistance;
        }

        private static int DurationMs(Event @event, int fallback = 5000)
        {
            return int.TryParse(@event["DurationMilliseconds"], out var d) && d > 0 ? d : fallback;
        }

        #endregion
    }
}
