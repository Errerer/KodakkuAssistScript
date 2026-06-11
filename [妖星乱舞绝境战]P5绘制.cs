using System;
using System.Numerics;
using KodakkuAssist.Module.Draw;
using KodakkuAssist.Module.GameEvent;
using KodakkuAssist.Script;
using Newtonsoft.Json;

namespace ErrerScriptNamespace
{
    [ScriptType(
        name: "[妖星乱舞绝境战]P5地火绘制",
        territorys: [1363],
        guid: "b3f7c1a2-8d4e-4f6a-9c12-5e8a1b3d7f90",
        version: "0.0.7",
        author: "Errer",
        note: "P5全套：地火步进圈 + 钢铁月环 + 核爆分散。\n" +
              "安全点暂注释，地火圈+钢铁月环+核爆分散正常。")]
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
        public int WaterDelayMs { get; set; } = 5000;

        [UserSetting("地水显示时长（ms）")]
        public int WaterDurationMs { get; set; } = 10000;

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
        private static readonly Vector3 ArenaCenter = new(100f, 0f, 100f);

        private int _fireCount;

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
            accessory.Method.RemoveDraw($"{DrawPrefix}_.*");
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
                    accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
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
                accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dpSafe);

                var dpGuide = accessory.Data.GetDefaultDrawProperties();
                dpGuide.Name = $"{DrawPrefix}_Guide{pairIndex}";
                dpGuide.Owner = accessory.Data.Me;
                dpGuide.TargetPosition = safePos;
                dpGuide.Scale = new Vector2(1.5f);
                dpGuide.ScaleMode = ScaleMode.YByDistance;
                dpGuide.Color = GuideColor.V4;
                dpGuide.Delay = safeDelay;
                dpGuide.DestoryAt = safeDestroy;
                accessory.Method.SendDraw(DrawModeEnum.Imgui, DrawTypeEnum.Displacement, dpGuide);

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
            accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
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
            accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dpSafe);

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
            accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Donut, dpDanger);
        }

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
            accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
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
            accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dp);
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
            accessory.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Rect, dp);

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
            a.Method.SendDraw(DrawModeEnum.Default, DrawTypeEnum.Circle, dpSafe);

            var dpGuide = a.Data.GetDefaultDrawProperties();
            dpGuide.Name = $"{DrawPrefix}_FloodGuide{idx}";
            dpGuide.Owner = a.Data.Me;
            dpGuide.TargetPosition = pos;
            dpGuide.Scale = new Vector2(1.5f);
            dpGuide.ScaleMode = ScaleMode.YByDistance;
            dpGuide.Color = new Vector4(0f, 1f, 1f, 1f);
            dpGuide.Delay = delayMs;
            dpGuide.DestoryAt = durationMs;
            a.Method.SendDraw(DrawModeEnum.Imgui, DrawTypeEnum.Displacement, dpGuide);
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
