using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValidationController : ControllerBase
{
    [HttpGet("IsAuth")]
    public IActionResult IsAuth()
    {
        //if (string.IsNullOrEmpty(Request.Headers.Cookie)) 
        //{
        //    return BadRequest();
        //}

        if (!User.Identity!.IsAuthenticated) // if user not Authenticated
            return Ok("false");

        return Ok("true");
    }

    [HttpGet("Logout")]
    [Authorize]
    public IActionResult LogOut()
    {
        Response.Headers.SetCookie =
            ".AspNetCore.Identity.Application=CfDJ8Mh4BCqnPF1FvqzvCaFzm655m1Fy5zcbzBmyfJ_Ht3GjRBvw_LNw0prGFD0kLHNywijuERtXt2imDZL4qZm3r9RuZAHxC5Eg6rX-tnsP-G_rp2RpKe-pX0Jc8n_3onkrA2KpB_VIVC9fl2vrhztyYFdfrlmtANkSGSGccIRvg46i0FonkKCPwQeWCPtUtiRUDIs84OeCdw3VWScm4AW5J9FB7D3zPD5htwd2xudBNR8cVL4FJKyzRdCztVTSeQHpZ9bPEMuXiNob9O36rM3MY3KM7NxlUImf-KYvsXxpLtWkR57VZWCEwwQTohcs_S1gb-kOgMzCXQMsHkUzUthD-UOv84-C-ab74LTdNfqb4Cg0jcAxheRGvSz9yVivudfrm89YQy4QTrapYU_J4DASPDAwoh6x6nakaft_JnWCSOsV2Pazvu_eP5IGCAmjqSWJ4jc8aK7AliTZUH2lRt0HWrfI5ZvYJPrjE7aa2-TAZeU0IlaCMu9XaZ5tlmp2syn1ZPGxGBKtb6_urdKaccrCNuxQ5LQGwEDOdLBkFvrtqsNNnq79-xEEATlI61OHTzS26k3G1C95jX-JiTggsGDLaPmqLk21G0PQY58P5GI8gQN2WAV0L0Cm7r7qTKBIocaCZkbuD8hCBQ-GzrRCAWaySQy9v7-TaciWxM9yiUMc6ztRtQXkfSozWrDsFONiKYOCBwGwwNyBbHHTCJadpjB2KAdjAX_aeg_wBhYm6_cdKo7n; expires=Tue, 23 Jan 2010 13:04:57 GMT; path=/; secure; samesite=none; httponly";
        return NoContent();
    }
}