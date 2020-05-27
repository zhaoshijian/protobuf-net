﻿using System;
using System.Diagnostics;

namespace ProtoBuf.Internal.Serializers
{
    internal sealed class DecimalSerializer : IRuntimeProtoSerializerNode
    {
        public static DecimalSerializer Create(CompatibilityLevel _)
            => s_Instance; // TODO - future support re https://github.com/protocolbuffers/protobuf/pull/7039

        private DecimalSerializer() { }
        private static readonly DecimalSerializer s_Instance = new DecimalSerializer();

        private static readonly Type expectedType = typeof(decimal);

        public Type ExpectedType => expectedType;

        bool IRuntimeProtoSerializerNode.RequiresOldValue => false;

        bool IRuntimeProtoSerializerNode.ReturnsValue => true;

        public object Read(ref ProtoReader.State state, object value)
        {
            Debug.Assert(value == null); // since replaces
            return BclHelpers.ReadDecimal(ref state);
        }

        public void Write(ref ProtoWriter.State state, object value)
        {
            BclHelpers.WriteDecimal(ref state, (decimal)value);
        }

        void IRuntimeProtoSerializerNode.EmitWrite(Compiler.CompilerContext ctx, Compiler.Local valueFrom)
        {
            ctx.EmitStateBasedWrite(nameof(BclHelpers.WriteDecimal), valueFrom, typeof(BclHelpers));
        }
        void IRuntimeProtoSerializerNode.EmitRead(Compiler.CompilerContext ctx, Compiler.Local entity)
        {
            ctx.EmitStateBasedRead(typeof(BclHelpers), nameof(BclHelpers.ReadDecimal), ExpectedType);
        }
    }
}