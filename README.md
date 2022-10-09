# DomainEvents

This sample shows how to use domain events. Let's imagine that we have an application which manages accounts and account groups. Account group can have many accounts, account can belong to many groups. If user removed an account then this account should be removed from all groups. If user removed the latest account from the group then group also removed.
- master branch contains two independent handlers with similar logic of removal account from account group
- command-from-command branch inoked RemoveAccountFromGroupCommand from DeleteAccountCommandHandler ([diff](https://github.com/denis-tsv/DomainEvents/pull/5/files))
- notification-from-command branch published AccountDeletedNotification and invoked RemoveAccountFromGroupCommand from notification handler ([diff](https://github.com/denis-tsv/DomainEvents/pull/6/files))
- domain-event branch publiched AccountDeletedNotification using universal notifications engine ([diff](https://github.com/denis-tsv/DomainEvents/pull/7/files))
- account-group-domain-events branc adds support of domain events to AccountGroup aggregate ([diff](https://github.com/denis-tsv/DomainEvents/pull/8/files))