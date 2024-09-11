// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial
// declarations in another file.
// </auto-generated>
#pragma warning disable 0109
#pragma warning disable 1591


namespace Quantum.Prototypes {
  using Photon.Deterministic;
  using Quantum;
  using Quantum.Core;
  using Quantum.Collections;
  using Quantum.Inspector;
  using Quantum.Physics2D;
  using Quantum.Physics3D;
  using Byte = System.Byte;
  using SByte = System.SByte;
  using Int16 = System.Int16;
  using UInt16 = System.UInt16;
  using Int32 = System.Int32;
  using UInt32 = System.UInt32;
  using Int64 = System.Int64;
  using UInt64 = System.UInt64;
  using Boolean = System.Boolean;
  using String = System.String;
  using Object = System.Object;
  using FlagsAttribute = System.FlagsAttribute;
  using SerializableAttribute = System.SerializableAttribute;
  using MethodImplAttribute = System.Runtime.CompilerServices.MethodImplAttribute;
  using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;
  using FieldOffsetAttribute = System.Runtime.InteropServices.FieldOffsetAttribute;
  using StructLayoutAttribute = System.Runtime.InteropServices.StructLayoutAttribute;
  using LayoutKind = System.Runtime.InteropServices.LayoutKind;
  #if QUANTUM_UNITY //;
  using TooltipAttribute = UnityEngine.TooltipAttribute;
  using HeaderAttribute = UnityEngine.HeaderAttribute;
  using SpaceAttribute = UnityEngine.SpaceAttribute;
  using RangeAttribute = UnityEngine.RangeAttribute;
  using HideInInspectorAttribute = UnityEngine.HideInInspector;
  using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;
  using FormerlySerializedAsAttribute = UnityEngine.Serialization.FormerlySerializedAsAttribute;
  using MovedFromAttribute = UnityEngine.Scripting.APIUpdating.MovedFromAttribute;
  using CreateAssetMenu = UnityEngine.CreateAssetMenuAttribute;
  using RuntimeInitializeOnLoadMethodAttribute = UnityEngine.RuntimeInitializeOnLoadMethodAttribute;
  #endif //;
  
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.Ability))]
  public unsafe partial class AbilityPrototype : StructPrototype {
    public AssetRef<AbilityData> AbilityData;
    partial void MaterializeUser(Frame frame, ref Quantum.Ability result, in PrototypeMaterializationContext context);
    public void Materialize(Frame frame, ref Quantum.Ability result, in PrototypeMaterializationContext context = default) {
        result.AbilityData = this.AbilityData;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.AbilityInventory))]
  public unsafe partial class AbilityInventoryPrototype : ComponentPrototype<Quantum.AbilityInventory> {
    [Header("Ability Order: Dash, OrbitalSupport, Return")]
    [ArrayLengthAttribute(3)]
    public Quantum.Prototypes.AbilityPrototype[] Abilities = new Quantum.Prototypes.AbilityPrototype[3];
    partial void MaterializeUser(Frame frame, ref Quantum.AbilityInventory result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.AbilityInventory component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.AbilityInventory result, in PrototypeMaterializationContext context = default) {
        for (int i = 0, count = PrototypeValidator.CheckLength(Abilities, 3, in context); i < count; ++i) {
          this.Abilities[i].Materialize(frame, ref *result.Abilities.GetPointer(i), in context);
        }
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.ActiveAbilityInfo))]
  public unsafe partial class ActiveAbilityInfoPrototype : StructPrototype {
    [HideInInspector()]
    public Int32 _empty_prototype_dummy_field_;
    partial void MaterializeUser(Frame frame, ref Quantum.ActiveAbilityInfo result, in PrototypeMaterializationContext context);
    public void Materialize(Frame frame, ref Quantum.ActiveAbilityInfo result, in PrototypeMaterializationContext context = default) {
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.BulletFields))]
  public unsafe class BulletFieldsPrototype : ComponentPrototype<Quantum.BulletFields> {
    public FP Time;
    public MapEntityId Source;
    public FPVector3 Direction;
    public AssetRef<BulletData> BulletData;
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.BulletFields component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.BulletFields result, in PrototypeMaterializationContext context = default) {
        result.Time = this.Time;
        PrototypeValidator.FindMapEntity(this.Source, in context, out result.Source);
        result.Direction = this.Direction;
        result.BulletData = this.BulletData;
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.CountdownTimer))]
  public unsafe partial class CountdownTimerPrototype : StructPrototype {
    public FP TimeLeft;
    public FP StartTime;
    partial void MaterializeUser(Frame frame, ref Quantum.CountdownTimer result, in PrototypeMaterializationContext context);
    public void Materialize(Frame frame, ref Quantum.CountdownTimer result, in PrototypeMaterializationContext context = default) {
        result.TimeLeft = this.TimeLeft;
        result.StartTime = this.StartTime;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.GameController))]
  public unsafe partial class GameControllerPrototype : StructPrototype {
    public Quantum.QEnum32<GameState> State;
    public FP GameTimer;
    partial void MaterializeUser(Frame frame, ref Quantum.GameController result, in PrototypeMaterializationContext context);
    public void Materialize(Frame frame, ref Quantum.GameController result, in PrototypeMaterializationContext context = default) {
        result.State = this.State;
        result.GameTimer = this.GameTimer;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.Input))]
  public unsafe partial class InputPrototype : StructPrototype {
    public Byte MovementEncoded;
    public Button MouseLeftButton;
    public FP ScreenPositionX;
    public FP ScreenPositionY;
    public Button MainWeaponFire;
    public Button FirstSkill;
    public Button SecondSkill;
    public Button ThirdSkill;
    public Button FourthSkill;
    public Button FifthSkill;
    public Button SixthSkill;
    public Button SeventhSkill;
    public Button EighthSkill;
    public Button NinthSkill;
    public Button TenthSkill;
    public Button Return;
    partial void MaterializeUser(Frame frame, ref Quantum.Input result, in PrototypeMaterializationContext context);
    public void Materialize(Frame frame, ref Quantum.Input result, in PrototypeMaterializationContext context = default) {
        result.MovementEncoded = this.MovementEncoded;
        result.MouseLeftButton = this.MouseLeftButton;
        result.ScreenPositionX = this.ScreenPositionX;
        result.ScreenPositionY = this.ScreenPositionY;
        result.MainWeaponFire = this.MainWeaponFire;
        result.FirstSkill = this.FirstSkill;
        result.SecondSkill = this.SecondSkill;
        result.ThirdSkill = this.ThirdSkill;
        result.FourthSkill = this.FourthSkill;
        result.FifthSkill = this.FifthSkill;
        result.SixthSkill = this.SixthSkill;
        result.SeventhSkill = this.SeventhSkill;
        result.EighthSkill = this.EighthSkill;
        result.NinthSkill = this.NinthSkill;
        result.TenthSkill = this.TenthSkill;
        result.Return = this.Return;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.KCC))]
  public unsafe class KCCPrototype : ComponentPrototype<Quantum.KCC> {
    public AssetRef<KCCSettings> Settings;
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.KCC component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.KCC result, in PrototypeMaterializationContext context = default) {
        result.Settings = this.Settings;
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.KCCCollision))]
  public unsafe class KCCCollisionPrototype : StructPrototype {
    public Quantum.QEnum8<EKCCCollisionSource> Source;
    public MapEntityId Reference;
    public AssetRef Processor;
    public void Materialize(Frame frame, ref Quantum.KCCCollision result, in PrototypeMaterializationContext context = default) {
        result.Source = this.Source;
        PrototypeValidator.FindMapEntity(this.Reference, in context, out result.Reference);
        result.Processor = this.Processor;
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.KCCData))]
  public unsafe partial class KCCDataPrototype : StructPrototype {
    public QBoolean IsActive;
    public FP LookPitch;
    public FP LookYaw;
    public FPVector3 BasePosition;
    public FPVector3 DesiredPosition;
    public FPVector3 TargetPosition;
    public FP DeltaTime;
    public FPVector3 InputDirection;
    public FPVector3 JumpImpulse;
    public FPVector3 Gravity;
    public FP MaxGroundAngle;
    public FP MaxWallAngle;
    public FP MaxHangAngle;
    public FPVector3 ExternalImpulse;
    public FPVector3 ExternalForce;
    public FPVector3 ExternalDelta;
    public FP KinematicSpeed;
    public FPVector3 KinematicTangent;
    public FPVector3 KinematicDirection;
    public FPVector3 KinematicVelocity;
    public FPVector3 DynamicVelocity;
    public FP RealSpeed;
    public FPVector3 RealVelocity;
    public QBoolean HasJumped;
    public QBoolean HasTeleported;
    public QBoolean IsGrounded;
    public QBoolean WasGrounded;
    public QBoolean IsSteppingUp;
    public QBoolean WasSteppingUp;
    public QBoolean IsSnappingToGround;
    public QBoolean WasSnappingToGround;
    public FPVector3 GroundNormal;
    public FPVector3 GroundTangent;
    public FPVector3 GroundPosition;
    public FP GroundDistance;
    public FP GroundAngle;
    partial void MaterializeUser(Frame frame, ref Quantum.KCCData result, in PrototypeMaterializationContext context);
    public void Materialize(Frame frame, ref Quantum.KCCData result, in PrototypeMaterializationContext context = default) {
        result.IsActive = this.IsActive;
        result.LookPitch = this.LookPitch;
        result.LookYaw = this.LookYaw;
        result.BasePosition = this.BasePosition;
        result.DesiredPosition = this.DesiredPosition;
        result.TargetPosition = this.TargetPosition;
        result.DeltaTime = this.DeltaTime;
        result.InputDirection = this.InputDirection;
        result.JumpImpulse = this.JumpImpulse;
        result.Gravity = this.Gravity;
        result.MaxGroundAngle = this.MaxGroundAngle;
        result.MaxWallAngle = this.MaxWallAngle;
        result.MaxHangAngle = this.MaxHangAngle;
        result.ExternalImpulse = this.ExternalImpulse;
        result.ExternalForce = this.ExternalForce;
        result.ExternalDelta = this.ExternalDelta;
        result.KinematicSpeed = this.KinematicSpeed;
        result.KinematicTangent = this.KinematicTangent;
        result.KinematicDirection = this.KinematicDirection;
        result.KinematicVelocity = this.KinematicVelocity;
        result.DynamicVelocity = this.DynamicVelocity;
        result.RealSpeed = this.RealSpeed;
        result.RealVelocity = this.RealVelocity;
        result.HasJumped = this.HasJumped;
        result.HasTeleported = this.HasTeleported;
        result.IsGrounded = this.IsGrounded;
        result.WasGrounded = this.WasGrounded;
        result.IsSteppingUp = this.IsSteppingUp;
        result.WasSteppingUp = this.WasSteppingUp;
        result.IsSnappingToGround = this.IsSnappingToGround;
        result.WasSnappingToGround = this.WasSnappingToGround;
        result.GroundNormal = this.GroundNormal;
        result.GroundTangent = this.GroundTangent;
        result.GroundPosition = this.GroundPosition;
        result.GroundDistance = this.GroundDistance;
        result.GroundAngle = this.GroundAngle;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.KCCIgnore))]
  public unsafe class KCCIgnorePrototype : StructPrototype {
    public Quantum.QEnum8<EKCCIgnoreSource> Source;
    public MapEntityId Reference;
    public void Materialize(Frame frame, ref Quantum.KCCIgnore result, in PrototypeMaterializationContext context = default) {
        result.Source = this.Source;
        PrototypeValidator.FindMapEntity(this.Reference, in context, out result.Reference);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.KCCModifier))]
  public unsafe class KCCModifierPrototype : StructPrototype {
    public AssetRef Processor;
    public MapEntityId Entity;
    public void Materialize(Frame frame, ref Quantum.KCCModifier result, in PrototypeMaterializationContext context = default) {
        result.Processor = this.Processor;
        PrototypeValidator.FindMapEntity(this.Entity, in context, out result.Entity);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.KCCProcessorLink))]
  public unsafe partial class KCCProcessorLinkPrototype : ComponentPrototype<Quantum.KCCProcessorLink> {
    public AssetRef<KCCProcessor> Processor;
    partial void MaterializeUser(Frame frame, ref Quantum.KCCProcessorLink result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.KCCProcessorLink component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.KCCProcessorLink result, in PrototypeMaterializationContext context = default) {
        result.Processor = this.Processor;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.MechProjectile))]
  public unsafe class MechProjectilePrototype : ComponentPrototype<Quantum.MechProjectile> {
    public FP TTL;
    public MapEntityId Owner;
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.MechProjectile component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.MechProjectile result, in PrototypeMaterializationContext context = default) {
        result.TTL = this.TTL;
        PrototypeValidator.FindMapEntity(this.Owner, in context, out result.Owner);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.Nexus))]
  public unsafe partial class NexusPrototype : ComponentPrototype<Quantum.Nexus> {
    public Quantum.QEnum32<Team> Team;
    public FP CurrentHealth;
    public QBoolean IsDestroy;
    public QBoolean IsTeamDefeat;
    partial void MaterializeUser(Frame frame, ref Quantum.Nexus result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.Nexus component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.Nexus result, in PrototypeMaterializationContext context = default) {
        result.Team = this.Team;
        result.CurrentHealth = this.CurrentHealth;
        result.IsDestroy = this.IsDestroy;
        result.IsTeamDefeat = this.IsTeamDefeat;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.PlayableMechanic))]
  public unsafe partial class PlayableMechanicPrototype : ComponentPrototype<Quantum.PlayableMechanic> {
    public Quantum.QEnum32<Team> Team;
    public Quantum.Prototypes.SkillPrototype ReturnSkill;
    partial void MaterializeUser(Frame frame, ref Quantum.PlayableMechanic result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.PlayableMechanic component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.PlayableMechanic result, in PrototypeMaterializationContext context = default) {
        result.Team = this.Team;
        this.ReturnSkill.Materialize(frame, ref result.ReturnSkill, in context);
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.PlayerData))]
  public unsafe partial class PlayerDataPrototype : StructPrototype {
    public QBoolean ready;
    partial void MaterializeUser(Frame frame, ref Quantum.PlayerData result, in PrototypeMaterializationContext context);
    public void Materialize(Frame frame, ref Quantum.PlayerData result, in PrototypeMaterializationContext context = default) {
        result.ready = this.ready;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.PlayerLink))]
  public unsafe partial class PlayerLinkPrototype : ComponentPrototype<Quantum.PlayerLink> {
    public PlayerRef PlayerRef;
    partial void MaterializeUser(Frame frame, ref Quantum.PlayerLink result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.PlayerLink component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.PlayerLink result, in PrototypeMaterializationContext context = default) {
        result.PlayerRef = this.PlayerRef;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.Skill))]
  public unsafe partial class SkillPrototype : StructPrototype {
    public Quantum.QEnum32<SkillStatus> Status;
    public FP RemainingCastingTime;
    public FP RemainingCoolTime;
    public AssetRef<SkillData> SkillData;
    partial void MaterializeUser(Frame frame, ref Quantum.Skill result, in PrototypeMaterializationContext context);
    public void Materialize(Frame frame, ref Quantum.Skill result, in PrototypeMaterializationContext context = default) {
        result.Status = this.Status;
        result.RemainingCastingTime = this.RemainingCastingTime;
        result.RemainingCoolTime = this.RemainingCoolTime;
        result.SkillData = this.SkillData;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.SkillInventory))]
  public unsafe partial class SkillInventoryPrototype : ComponentPrototype<Quantum.SkillInventory> {
    [DynamicCollectionAttribute()]
    public Quantum.Prototypes.SkillPrototype[] Skills = {};
    partial void MaterializeUser(Frame frame, ref Quantum.SkillInventory result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.SkillInventory component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.SkillInventory result, in PrototypeMaterializationContext context = default) {
        if (this.Skills.Length == 0) {
          result.Skills = default;
        } else {
          var list = frame.AllocateList(out result.Skills, this.Skills.Length);
          for (int i = 0; i < this.Skills.Length; ++i) {
            Quantum.Skill tmp = default;
            this.Skills[i].Materialize(frame, ref tmp, in context);
            list.Add(tmp);
          }
        }
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.SpawnIdentifier))]
  public unsafe partial class SpawnIdentifierPrototype : ComponentPrototype<Quantum.SpawnIdentifier> {
    public Quantum.QEnum32<Team> Team;
    partial void MaterializeUser(Frame frame, ref Quantum.SpawnIdentifier result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.SpawnIdentifier component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.SpawnIdentifier result, in PrototypeMaterializationContext context = default) {
        result.Team = this.Team;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.Status))]
  public unsafe partial class StatusPrototype : ComponentPrototype<Quantum.Status> {
    public FP CurrentHealth;
    public QBoolean IsDead;
    public FP RespawnTimer;
    public FP RegenTimer;
    public FP InvincibleTimer;
    public Int32 DisconnectedTicks;
    public AssetRef<StatusData> StatusData;
    public AssetRef<PlayerMovementData> PlayerMovementData;
    partial void MaterializeUser(Frame frame, ref Quantum.Status result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.Status component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.Status result, in PrototypeMaterializationContext context = default) {
        result.CurrentHealth = this.CurrentHealth;
        result.IsDead = this.IsDead;
        result.RespawnTimer = this.RespawnTimer;
        result.RegenTimer = this.RegenTimer;
        result.InvincibleTimer = this.InvincibleTimer;
        result.DisconnectedTicks = this.DisconnectedTicks;
        result.StatusData = this.StatusData;
        result.PlayerMovementData = this.PlayerMovementData;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.Weapon))]
  public unsafe partial class WeaponPrototype : StructPrototype {
    public QBoolean IsRecharging;
    public Int32 CurrentAmmo;
    public FP FireRateTimer;
    public FP DelayToStartRechargeTimer;
    public FP RechargeRate;
    public FP ChargeTime;
    public AssetRef<PrimaryWeaponData> WeaponData;
    partial void MaterializeUser(Frame frame, ref Quantum.Weapon result, in PrototypeMaterializationContext context);
    public void Materialize(Frame frame, ref Quantum.Weapon result, in PrototypeMaterializationContext context = default) {
        result.IsRecharging = this.IsRecharging;
        result.CurrentAmmo = this.CurrentAmmo;
        result.FireRateTimer = this.FireRateTimer;
        result.DelayToStartRechargeTimer = this.DelayToStartRechargeTimer;
        result.RechargeRate = this.RechargeRate;
        result.ChargeTime = this.ChargeTime;
        result.WeaponData = this.WeaponData;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.WeaponInventory))]
  public unsafe partial class WeaponInventoryPrototype : ComponentPrototype<Quantum.WeaponInventory> {
    public Int32 CurrentWeaponIndex;
    [DynamicCollectionAttribute()]
    public Quantum.Prototypes.WeaponPrototype[] Weapons = {};
    partial void MaterializeUser(Frame frame, ref Quantum.WeaponInventory result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.WeaponInventory component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.WeaponInventory result, in PrototypeMaterializationContext context = default) {
        result.CurrentWeaponIndex = this.CurrentWeaponIndex;
        if (this.Weapons.Length == 0) {
          result.Weapons = default;
        } else {
          var list = frame.AllocateList(out result.Weapons, this.Weapons.Length);
          for (int i = 0; i < this.Weapons.Length; ++i) {
            Quantum.Weapon tmp = default;
            this.Weapons[i].Materialize(frame, ref tmp, in context);
            list.Add(tmp);
          }
        }
        MaterializeUser(frame, ref result, in context);
    }
  }
}
#pragma warning restore 0109
#pragma warning restore 1591
