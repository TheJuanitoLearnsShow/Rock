﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
namespace Rock.Migrations
{
    /// <summary>
    ///
    /// </summary>
    public partial class Rollup_20230907 : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            ResetBlockSettingGroupAttendanceDetailListItemDetailsTemplate();
            UpdatePersonAgeAndAgeBracket();
            UpdateInteractionComponentIndex();
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
        }

        /// <summary>
        /// JE: Update InteractionComponent Index
        /// </summary>
        private void UpdateInteractionComponentIndex()
        {
            Sql( @"-- Drop original index
DROP INDEX IF EXISTS [IX_InteractionChannelId] ON [InteractionComponent]


-- Drop new index (just in case it exists)
DROP INDEX IF EXISTS [IX_ChannelId_EntityId] ON [InteractionComponent]

-- Create new index
CREATE NONCLUSTERED INDEX [IX_ChannelId_EntityId] ON [dbo].[InteractionComponent]
(
    [InteractionChannelId] ASC,
    [EntityId] ASC
    
)
INCLUDE([Guid])
WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]" );
        }

        /// <summary>
        /// JMH: Reset Block Setting: Group Attendance Detail - List Item Details Template
        /// </summary>
        private void ResetBlockSettingGroupAttendanceDetailListItemDetailsTemplate()
        {
            Sql( @"
DECLARE @EntityTypeId_BlockType INT = (SELECT [Id] FROM [EntityType] WHERE [Name] = 'Rock.Model.Block')
DECLARE @BlockTypeId_GroupAttendanceDetail INT = (SELECT [Id] FROM [BlockType] WHERE [Guid] = '308DBA32-F656-418E-A019-9D18235027C1')
DECLARE @AttributeKey_ListItemDetailsTemplate NVARCHAR(1000) = N'ListItemDetailsTemplate'

UPDATE [AttributeValue]
   SET [Value] = '',
       [IsPersistedValueDirty] = 1
 WHERE [AttributeId] IN (
    SELECT [Id]
      FROM [Attribute]
     WHERE [EntityTypeId] = @EntityTypeId_BlockType
           AND [EntityTypeQualifierColumn] = 'BlockTypeId'
           AND [EntityTypeQualifierValue] = @BlockTypeId_GroupAttendanceDetail
           AND [Key] = @AttributeKey_ListItemDetailsTemplate
)
" );
        }

        /// <summary>
        /// KA: Migration to update Person Age and AgeBracket
        /// </summary>
        private void UpdatePersonAgeAndAgeBracket()
        {

            Sql( @"
BEGIN
	UPDATE Person
	SET [BirthDateKey] = FORMAT([BirthDate],'yyyyMMdd')

	UPDATE P
	SET P.[Age] = CASE
		WHEN P.[DeceasedDate] IS NOT NULL THEN
		DATEDIFF(YEAR, A.[Date], P.[DeceasedDate]) - 
			CASE 
				WHEN DATEADD(YY, DATEDIFF(yy, A.[Date], P.[DeceasedDate]), P.[DeceasedDate]) > p.[DeceasedDate] THEN 1
				ELSE 0
				END
		WHEN p.[BirthDate] IS NULL THEN NULL
		ELSE A.[Age] 
		END,
	P.[AgeBracket] = CASE
        WHEN A.[AgeBracket] IS NULL THEN 0
        ELSE A.[AgeBracket]
        END        
	FROM Person P
	LEFT JOIN AnalyticsSourceDate A
	ON A.[DateKey] = P.[BirthDateKey]
END
" );
        }
    }
}
