﻿namespace GalacticWasteManagement.Scripts.ScriptProviders
{
    public class DropSchema : ScriptBase
    {
        public override string Sql => @"
declare @n char(1)
set @n = char(10)

declare @stmt nvarchar(max)

select @stmt = isnull( @stmt + @n, '' ) +
    'drop procedure [' + name + ']'
from sys.procedures

-- check constraints
select @stmt = isnull( @stmt + @n, '' ) +
    'alter table [' + object_name( parent_object_id ) + '] drop constraint [' + name + ']'
from sys.check_constraints

-- functions
select @stmt = isnull( @stmt + @n, '' ) +
    'drop function [' + name + ']'
from sys.objects
where type in ( 'FN', 'IF', 'TF' )

-- views
select @stmt = isnull( @stmt + @n, '' ) +
    'drop view [' + name + ']'
from sys.views

-- foreign keys
select @stmt = isnull( @stmt + @n, '' ) +
    'alter table [' + object_name( parent_object_id ) + '] drop constraint [' + name + ']'
from sys.foreign_keys

-- tables
select @stmt = isnull( @stmt + @n, '' ) +
    'drop table [' + name + ']'
from sys.tables

-- user defined types
select @stmt = isnull( @stmt + @n, '' ) +
    'drop type [' + name + ']'
from sys.types
where is_user_defined = 1

exec sp_executesql @stmt
";

        public override string Name => nameof(DropSchema);

        public override IScriptType Type => ScriptType.Drop;
    }
}
