CREATE TABLE [Secure].[Users] (
    [UserID]   INT           IDENTITY (1, 1) NOT NULL,
    [UserName] VARCHAR (50)  NOT NULL,
    [Email]    VARCHAR (320) NULL,
    [IsActive] BIT           CONSTRAINT [DF_Users_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_users\] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [UQ__Users__F3DBC5729FDACB2A] UNIQUE NONCLUSTERED ([UserName] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UniqueEmailUsers]
    ON [Secure].[Users]([Email] ASC) WHERE ([isactive]=(1));

