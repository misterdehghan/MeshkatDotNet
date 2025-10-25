BEGIN TRANSACTION;
GO

DROP TABLE [SurveyAnswerDescriptives];
GO

DROP TABLE [SurveyAnswers];
GO

DROP TABLE [SurveyPopulations];
GO

DROP TABLE [Surveys];
GO

DROP TABLE [SurveyQuestions];
GO

DROP TABLE [SurveyGroup];
GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20221231074244_survay';
GO

COMMIT;
GO

