namespace WebApi.Enums;

public enum ERemoveNovelUserResult
{
    Success,
    OwnerTryRemoveItself,
    AlreadyDontOwnPermission,
    ThisNovelUserIdNotExist
}