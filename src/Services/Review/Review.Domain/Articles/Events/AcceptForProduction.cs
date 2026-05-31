
using Articles.Abstractions;
using Blocks.Domain;

namespace Review.Domain.Articles.Events;

public  record AcceptForProduction(Article Article, Editor Editor, IArticleAction action) : IDomainEvent;
