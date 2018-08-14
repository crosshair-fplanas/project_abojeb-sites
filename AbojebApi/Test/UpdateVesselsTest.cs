using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbojebApi.Data.Services;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Collections;
using AbojebApi.Core.DataTransferObjects;
using AbojebApi.Core.Data;
using System.Collections.Generic;
using AbojebApi.Data;
using AutoMapper;
namespace Abojeb.Data.Test
{
    [TestClass]
    public class UpdateVesselsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var configuration1 = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<VesselProfile>();
            //});

            VesselDto[] vesselDtos = { new VesselDto{
                BoatName = "Name1",
                CallSign = "si",
                VesselType = "B"
            },new VesselDto{
                BoatName = "Name2",
                CallSign = "si",
                VesselType = "B"
            }};

            ReportDto[] reportDtos = { new ReportDto {
                IMO = 1
            },new ReportDto {
                IMO = 2
            }};

            List<Vessel> vessels = vesselDtos.Select(s => s.Convert<VesselDto, Vessel>()).ToList();
            List<Report> reports = reportDtos.Select(s => s.Convert<ReportDto, Report>()).ToList();
            //var svc = new ApiService();
            //svc.GetAllVessels();
        }
    }
}
