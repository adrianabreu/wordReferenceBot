CREATE TABLE [dbo].[words] (
	Id bigint NOT NULL,
	Word nvarchar(200) NOT NULL,
	PRIMARY KEY (Id)
)

CREATE TABLE [dbo].[translations] (
	Id bigint NOT NULL,
	Expression nvarchar(200) NOT NULL,
	WordId bigint NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (WordId) REFERENCES [dbo].[words](id) ON DELETE CASCADE
)

CREATE TABLE [dbo].[meanings] (
	Id bigint NOT NULL,
	[Value] nvarchar(200) NOT NULL,
	TranslationId bigint NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (Translationid) REFERENCES [dbo].[translations] ON DELETE CASCADE
)

CREATE TABLE [dbo].[possibletranslations] (
	Id bigint NOT NULL,
	[Value] nvarchar(200) NOT NULL,
	TranslationId bigint NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (Translationid) REFERENCES [dbo].[translations] ON DELETE CASCADE
)