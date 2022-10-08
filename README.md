# DomainEvents

This sample shows how to use domain events. Let's imagine that we have an application which manages accounts and account groups. Account group can have many accounts, account can belong to many groups. If user removed an account then this account should be removed from all groups. If user removed the latest account from the group then group also removed.
- master branch contains two handlers with duplicated logic of removal account from account group
- no-domain-events branch removed code duplication using application service ([diff](https://github.com/denis-tsv/DomainEvents/pull/1/files))
- domain-events branch used events to remove an account from all groups ([diff](https://github.com/denis-tsv/DomainEvents/pull/2/files))
- domain-events branch invoked command from event handler to remove an account from each group ([diff](https://github.com/denis-tsv/DomainEvents/pull/3/files))
- no-domain-events-fast branch contains fast inplementation using batch update and delete ([diff](https://github.com/denis-tsv/DomainEvents/pull/4/files))