namespace Web.Enums;

public enum ERemoveNovelUser //why enum Repeated in Web And WebApi : I Dont Want To Create Other Lib For This Only
{
    Success,
    OwnerTryRemoveItself,
    AlreadyDontOwnPermission,
    ThisNovelUserIdNotExist,
    NoPermission,
    ServerError,
    UnknownError
}