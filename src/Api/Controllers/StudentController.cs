﻿using Api.Constants;
using Application.Interface.Service;
using Domain.Model.Response;
using Domain.Model.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    [HttpGet(ApiEndPointConstant.Student.StudentGetListEndpoint)]
    public async Task<IActionResult> GetListStudentAsync([FromQuery] StudentSearchModel searchModel)
    {
        var result = await _studentService.GetListStudentAsync(searchModel);
        return Ok(result);
    }
    [HttpGet(ApiEndPointConstant.Student.StudentEndpoint)]
    public async Task<IActionResult> GetStudentByIdAsync(Guid id)
    {
        var result = await _studentService.GetStudentByIdAsync(id);
        return Ok(result);
    }
    [HttpPost(ApiEndPointConstant.Student.StudentPostEndpoint)]
    public async Task<IActionResult> CreateStudentAsyns(StudentPostModel postModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _studentService.CreateStudentAsync(postModel);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut(ApiEndPointConstant.Student.StudentPutEndpoint)]
    public async Task<IActionResult> UpdateStudentAsync(StudentPutModel putModel, Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _studentService.UpdateStudentAsync(putModel, id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost(ApiEndPointConstant.Student.ImportStudentEndpoint)]
    public async Task<IActionResult> ImportFromJsonAsync([FromForm] StudentImportModel studentImportModel)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _studentService.ImportStudentsFromJsonAsync(studentImportModel);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete(ApiEndPointConstant.Student.StudentDeleteEndpoint)]
    public async Task<IActionResult> DeleteStudentAsync(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _studentService.DeleteStudentAsync(id);
            return Ok(result);
        }catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
