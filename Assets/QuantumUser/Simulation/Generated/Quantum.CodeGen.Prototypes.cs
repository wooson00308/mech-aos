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
  [Quantum.Prototypes.Prototype(typeof(Quantum.AsteroidsAsteroid))]
  public unsafe partial class AsteroidsAsteroidPrototype : ComponentPrototype<Quantum.AsteroidsAsteroid> {
    public AssetRef<EntityPrototype> ChildAsteroid;
    partial void MaterializeUser(Frame frame, ref Quantum.AsteroidsAsteroid result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.AsteroidsAsteroid component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.AsteroidsAsteroid result, in PrototypeMaterializationContext context = default) {
        result.ChildAsteroid = this.ChildAsteroid;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.AsteroidsPlayerLink))]
  public unsafe partial class AsteroidsPlayerLinkPrototype : ComponentPrototype<Quantum.AsteroidsPlayerLink> {
    public PlayerRef PlayerRef;
    partial void MaterializeUser(Frame frame, ref Quantum.AsteroidsPlayerLink result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.AsteroidsPlayerLink component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.AsteroidsPlayerLink result, in PrototypeMaterializationContext context = default) {
        result.PlayerRef = this.PlayerRef;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.AsteroidsProjectile))]
  public unsafe class AsteroidsProjectilePrototype : ComponentPrototype<Quantum.AsteroidsProjectile> {
    public FP TTL;
    public MapEntityId Owner;
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.AsteroidsProjectile component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.AsteroidsProjectile result, in PrototypeMaterializationContext context = default) {
        result.TTL = this.TTL;
        PrototypeValidator.FindMapEntity(this.Owner, in context, out result.Owner);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.AsteroidsShip))]
  public unsafe partial class AsteroidsShipPrototype : ComponentPrototype<Quantum.AsteroidsShip> {
    public FP AmmoCount;
    public FP FireInterval;
    public Int32 Score;
    partial void MaterializeUser(Frame frame, ref Quantum.AsteroidsShip result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.AsteroidsShip component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.AsteroidsShip result, in PrototypeMaterializationContext context = default) {
        result.AmmoCount = this.AmmoCount;
        result.FireInterval = this.FireInterval;
        result.Score = this.Score;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.AsteroidsShipRespawn))]
  public unsafe partial class AsteroidsShipRespawnPrototype : ComponentPrototype<Quantum.AsteroidsShipRespawn> {
    public FP RespawnTimer;
    partial void MaterializeUser(Frame frame, ref Quantum.AsteroidsShipRespawn result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.AsteroidsShipRespawn component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.AsteroidsShipRespawn result, in PrototypeMaterializationContext context = default) {
        result.RespawnTimer = this.RespawnTimer;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.Input))]
  public unsafe partial class InputPrototype : StructPrototype {
    public Button Fire;
    public Button Left;
    public Button Right;
    public Button Up;
    public Button Down;
    public Button MainWeaponFire;
    public Button SubWeaponFire;
    partial void MaterializeUser(Frame frame, ref Quantum.Input result, in PrototypeMaterializationContext context);
    public void Materialize(Frame frame, ref Quantum.Input result, in PrototypeMaterializationContext context = default) {
        result.Fire = this.Fire;
        result.Left = this.Left;
        result.Right = this.Right;
        result.Up = this.Up;
        result.Down = this.Down;
        result.MainWeaponFire = this.MainWeaponFire;
        result.SubWeaponFire = this.SubWeaponFire;
        MaterializeUser(frame, ref result, in context);
    }
  }
  [System.SerializableAttribute()]
  [Quantum.Prototypes.Prototype(typeof(Quantum.PlayableMechanic))]
  public unsafe partial class PlayableMechanicPrototype : ComponentPrototype<Quantum.PlayableMechanic> {
    [HideInInspector()]
    public Int32 _empty_prototype_dummy_field_;
    partial void MaterializeUser(Frame frame, ref Quantum.PlayableMechanic result, in PrototypeMaterializationContext context);
    public override Boolean AddToEntity(FrameBase f, EntityRef entity, in PrototypeMaterializationContext context) {
        Quantum.PlayableMechanic component = default;
        Materialize((Frame)f, ref component, in context);
        return f.Set(entity, component) == SetResult.ComponentAdded;
    }
    public void Materialize(Frame frame, ref Quantum.PlayableMechanic result, in PrototypeMaterializationContext context = default) {
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
  [Quantum.Prototypes.Prototype(typeof(Quantum.Status))]
  public unsafe partial class StatusPrototype : ComponentPrototype<Quantum.Status> {
    public FP CurrentHealth;
    public QBoolean IsDead;
    public FP RespawnTimer;
    public FP RegenTimer;
    public FP InvincibleTimer;
    public Int32 DisconnectedTicks;
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
        MaterializeUser(frame, ref result, in context);
    }
  }
}
#pragma warning restore 0109
#pragma warning restore 1591
