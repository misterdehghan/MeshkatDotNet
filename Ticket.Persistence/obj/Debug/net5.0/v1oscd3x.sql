IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Description] nvarchar(max) NULL,
        [ParentId] nvarchar(450) NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoles_AspNetRoles_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [Groups] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [ParentId] bigint NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_Groups] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Groups_Groups_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Groups] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [Persons] (
        [Id] bigint NOT NULL IDENTITY,
        [personeli] nvarchar(max) NULL,
        [name] nvarchar(max) NULL,
        [family] nvarchar(max) NULL,
        [name_father] nvarchar(max) NULL,
        [tavalod] nvarchar(max) NULL,
        [melli] nvarchar(max) NULL,
        [she] nvarchar(max) NULL,
        [yegan] nvarchar(max) NULL,
        [yegan_r] nvarchar(max) NULL,
        [darajeh] nvarchar(max) NULL,
        CONSTRAINT [PK_Persons] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [QuizStartTemps] (
        [Id] bigint NOT NULL IDENTITY,
        [QuizId] bigint NOT NULL,
        [UserName] nvarchar(max) NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [Ip] nvarchar(max) NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_QuizStartTemps] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [WorkPlaces] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [ParentId] bigint NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_WorkPlaces] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WorkPlaces_WorkPlaces_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [WorkPlaces] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FirstName] nvarchar(50) NULL,
        [LastName] nvarchar(50) NULL,
        [Phone] nvarchar(11) NULL,
        [melli] nvarchar(max) NULL,
        [name_father] nvarchar(max) NULL,
        [tavalod] nvarchar(max) NULL,
        [GroupId] bigint NULL,
        [TypeDarajeh] int NOT NULL,
        [darajeh] int NOT NULL,
        [WorkPlaceId] bigint NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUsers_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AspNetUsers_WorkPlaces_WorkPlaceId] FOREIGN KEY ([WorkPlaceId]) REFERENCES [WorkPlaces] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        [UserId1] nvarchar(450) NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId1] FOREIGN KEY ([UserId1]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [GroupUsers] (
        [Id] bigint NOT NULL IDENTITY,
        [UserId] nvarchar(450) NULL,
        [GroupId] bigint NOT NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_GroupUsers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GroupUsers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_GroupUsers_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [Quizzes] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [Timer] int NULL,
        [StartDate] datetime2 NULL,
        [EndDate] datetime2 NULL,
        [CreatorId] nvarchar(450) NULL,
        [GroupId] bigint NOT NULL,
        [Password] nvarchar(max) NULL,
        [IsPrivate] bit NOT NULL,
        [MaxQuestion] int NOT NULL,
        [PasswordddId] bigint NULL,
        [QuizFilterId] bigint NULL,
        [QuizId] bigint NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_Quizzes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Quizzes_AspNetUsers_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Quizzes_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Quizzes_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quizzes] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [Passwords] (
        [Id] bigint NOT NULL IDENTITY,
        [Content] nvarchar(max) NULL,
        [QuizId] bigint NOT NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_Passwords] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Passwords_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quizzes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [Qestions] (
        [Id] bigint NOT NULL IDENTITY,
        [Text] nvarchar(max) NULL,
        [QuizId] bigint NOT NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_Qestions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Qestions_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quizzes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [QuizFilters] (
        [Id] bigint NOT NULL IDENTITY,
        [WorkpalceOption] nvarchar(max) NULL,
        [TypeDarajeh] int NULL,
        [UserNameOption] nvarchar(max) NULL,
        [QuizId] bigint NOT NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_QuizFilters] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuizFilters_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quizzes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [Results] (
        [Id] bigint NOT NULL IDENTITY,
        [Points] int NOT NULL,
        [MaxPoints] int NOT NULL,
        [AuthorizationResult] nvarchar(max) NULL,
        [AnsweresInQuiz] nvarchar(max) NULL,
        [StartQuiz] datetime2 NOT NULL,
        [EndQuiz] datetime2 NULL,
        [QuizId] bigint NOT NULL,
        [StudentId] nvarchar(450) NULL,
        [Ip] nvarchar(max) NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_Results] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Results_AspNetUsers_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Results_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quizzes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [Answers] (
        [Id] bigint NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [QuestionId] bigint NOT NULL,
        [Text] nvarchar(max) NULL,
        [IsTrue] bit NOT NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_Answers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Answers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Question_AnswerQuestions_onetomany] FOREIGN KEY ([QuestionId]) REFERENCES [Qestions] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE TABLE [Attachments] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [Url] nvarchar(max) NULL,
        [TempUrl] nvarchar(max) NULL,
        [UserId] nvarchar(450) NULL,
        [QuestionId] bigint NOT NULL,
        [AnswerId] bigint NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_Attachments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Attachments_Answers_AnswerId] FOREIGN KEY ([AnswerId]) REFERENCES [Answers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Attachments_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Attachments_Qestions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Qestions] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Answers_QuestionId] ON [Answers] ([QuestionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Answers_UserId] ON [Answers] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_AspNetRoles_ParentId] ON [AspNetRoles] ([ParentId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_UserId1] ON [AspNetUserRoles] ([UserId1]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_AspNetUsers_GroupId] ON [AspNetUsers] ([GroupId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_AspNetUsers_WorkPlaceId] ON [AspNetUsers] ([WorkPlaceId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Attachments_AnswerId] ON [Attachments] ([AnswerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Attachments_QuestionId] ON [Attachments] ([QuestionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Attachments_UserId] ON [Attachments] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Groups_ParentId] ON [Groups] ([ParentId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_GroupUsers_GroupId] ON [GroupUsers] ([GroupId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_GroupUsers_UserId] ON [GroupUsers] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE UNIQUE INDEX [IX_Passwords_QuizId] ON [Passwords] ([QuizId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Qestions_QuizId] ON [Qestions] ([QuizId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE UNIQUE INDEX [IX_QuizFilters_QuizId] ON [QuizFilters] ([QuizId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Quizzes_CreatorId] ON [Quizzes] ([CreatorId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Quizzes_GroupId] ON [Quizzes] ([GroupId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Quizzes_QuizId] ON [Quizzes] ([QuizId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Results_QuizId] ON [Results] ([QuizId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_Results_StudentId] ON [Results] ([StudentId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    CREATE INDEX [IX_WorkPlaces_ParentId] ON [WorkPlaces] ([ParentId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220519100320_initDb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220519100320_initDb', N'5.0.14');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220522045904_editworkplace')
BEGIN
    ALTER TABLE [WorkPlaces] ADD [SortIndex] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220522045904_editworkplace')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220522045904_editworkplace', N'5.0.14');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE TABLE [SurveyGroup] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [ParentId] int NULL,
        [ParentId1] bigint NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_SurveyGroup] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SurveyGroup_Groups_ParentId1] FOREIGN KEY ([ParentId1]) REFERENCES [Groups] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE TABLE [SurveyPopulations] (
        [Id] bigint NOT NULL IDENTITY,
        [Gender] bit NOT NULL,
        [KhedmatAgeRange] int NOT NULL,
        [TypeDarajeh] int NOT NULL,
        [darajeh] int NOT NULL,
        [Education] int NOT NULL,
        [WorkPlaceId] bigint NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_SurveyPopulations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SurveyPopulations_WorkPlaces_WorkPlaceId] FOREIGN KEY ([WorkPlaceId]) REFERENCES [WorkPlaces] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE TABLE [SurveyQuestions] (
        [Id] bigint NOT NULL IDENTITY,
        [Text] nvarchar(max) NULL,
        [QuestionType] int NOT NULL,
        [SurveyGroupId] bigint NOT NULL,
        [SurveyGroupId1] int NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_SurveyQuestions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SurveyQuestions_SurveyGroup_SurveyGroupId1] FOREIGN KEY ([SurveyGroupId1]) REFERENCES [SurveyGroup] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE TABLE [Surveys] (
        [Id] int NOT NULL IDENTITY,
        [SurveyGroupId] int NOT NULL,
        [CreatorId] nvarchar(450) NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_Surveys] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Surveys_AspNetUsers_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Surveys_SurveyGroup_SurveyGroupId] FOREIGN KEY ([SurveyGroupId]) REFERENCES [SurveyGroup] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE TABLE [SurveyAnswerDescriptives] (
        [Id] bigint NOT NULL IDENTITY,
        [SurveyQuestionId] bigint NOT NULL,
        [Title] nvarchar(max) NULL,
        [Index] int NOT NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_SurveyAnswerDescriptives] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SurveyAnswerDescriptives_SurveyQuestions_SurveyQuestionId] FOREIGN KEY ([SurveyQuestionId]) REFERENCES [SurveyQuestions] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE TABLE [SurveyAnswers] (
        [Id] bigint NOT NULL IDENTITY,
        [SurveyQuestionId] bigint NOT NULL,
        [Wight] int NOT NULL,
        [Title] nvarchar(max) NULL,
        [Index] int NOT NULL,
        [Status] tinyint NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [RegesterAt] datetime2 NULL,
        CONSTRAINT [PK_SurveyAnswers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SurveyAnswers_SurveyQuestions_SurveyQuestionId] FOREIGN KEY ([SurveyQuestionId]) REFERENCES [SurveyQuestions] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE INDEX [IX_SurveyAnswerDescriptives_SurveyQuestionId] ON [SurveyAnswerDescriptives] ([SurveyQuestionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE INDEX [IX_SurveyAnswers_SurveyQuestionId] ON [SurveyAnswers] ([SurveyQuestionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE INDEX [IX_SurveyGroup_ParentId1] ON [SurveyGroup] ([ParentId1]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE INDEX [IX_SurveyPopulations_WorkPlaceId] ON [SurveyPopulations] ([WorkPlaceId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE INDEX [IX_SurveyQuestions_SurveyGroupId1] ON [SurveyQuestions] ([SurveyGroupId1]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE INDEX [IX_Surveys_CreatorId] ON [Surveys] ([CreatorId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    CREATE INDEX [IX_Surveys_SurveyGroupId] ON [Surveys] ([SurveyGroupId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221231074244_survay')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221231074244_survay', N'5.0.14');
END;
GO

COMMIT;
GO

