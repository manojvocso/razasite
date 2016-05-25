using System.Web.Mvc;
using AutoMapper;
using MvcApplication1.AppHelper;
using MvcApplication1.Areas.Mobile.Models;
using MvcApplication1.Areas.Mobile.ViewModels;
using Raza.Model;
using Raza.Model.PaymentProcessModel;

namespace MvcApplication1
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            //AutoMapper.Mapper.CreateMap<Book, BookViewModel>()
            //    .ForMember(dest => dest.Author,
            //               opts => opts.MapFrom(src => src.Author.Name));

            Mapper.CreateMap<OrderHistoricPlanInfoSnapshot, MyAccountPlanEntity>()
                .ForMember(dest => dest.OrderId, opts => opts.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.PlanName, opts => opts.MapFrom(src => src.PlanName))
                .ForMember(dest => dest.PlanPin, opts => opts.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.LastTransactionDate, opts => opts.MapFrom(src => src.TransactionDate.ToString("MM/dd/yyyy")))
                .ForMember(dest => dest.IsActivePlan, opts => opts.MapFrom(src => src.IsActivePlan));

            Mapper.CreateMap<OrderHistoricPlanInfoSnapshot, PlanInfoViewModel>();

            //Mapping for Billing Info
            Mapper.CreateMap<BillingInfo, BillingInfoViewModel>();
            Mapper.CreateMap<BillingInfoViewModel, BillingInfo>();

            Mapper.CreateMap<GetCard, GetCard>()
               .ForMember(dest => dest.MaskCardNumber,
                   opts => opts.MapFrom(src => src.CreditCardNumber.Remove(4, 8).Insert(4, "-XXXXXXXX-")));

            Mapper.CreateMap<GetCard, SelectListItem>()
                .ForMember(dest => dest.Text,
                    opts => opts.MapFrom(src => ControllerHelper.MaskedCreditCard(src.CreditCardNumber)))
                .ForMember(dest => dest.Value, opts => opts.MapFrom(src => src.CreditCardId));

            Mapper.CreateMap<EachRatePlan, ShopCartMobileModel>()
                .ForMember(dest => dest.Amount, opts => opts.MapFrom(src => src.PlanAmount))
                .ForMember(dest => dest.PlanName, opts => opts.MapFrom(src => src.CardTypeName))
                .ForMember(dest => dest.ServiceFee, opts => opts.MapFrom(src => src.Discount));


            //Mapper.CreateMap<OrderHistoricPlanInfoSnapshot, ProcessPlanInfo>()
            //    .ForMember(dest => dest.CountryFrom, opts => opts.MapFrom(src => src.CountryFrom))
            //    .ForMember(dest => dest.CountryTo, opts => opts.MapFrom(src => src.CountryTo))
            //    .ForMember(dest => dest.Pin, opts => opts.MapFrom(src => src.AccountNumber))
            //    .ForMember(dest => dest.OrderId, opts => opts.MapFrom(src => src.OrderId))
            //    .ForMember(dest => dest.CardId, opts => opts.MapFrom(src => src.PlanId))
            //    .ForMember(dest => dest.CurrencyCode, opts => opts.MapFrom(src => src.CurrencyCode))
            //    .ForMember(dest => dest.PlanName, opts => opts.MapFrom(src => src.PlanName))
                


        }
    }
}