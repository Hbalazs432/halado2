using AutoMapper;
using crypto_s4buby.Context.Dtos;
using crypto_s4buby.Context.Entities;

namespace crypto_s4buby.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User,UserDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserLoginDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<WalletDto, Wallet>().ReverseMap();
            CreateMap<WalletUpdateDto, Wallet>();
            CreateMap<CryptoDto, Crypto>().ReverseMap();
            CreateMap<Crypto, CryptoUpdateDto>();
            CreateMap<CryptoHistoryDto, CryptoUpdateDto>().ReverseMap();
            CreateMap<CryptoHistory, CryptoHistoryDto>().ReverseMap();
            CreateMap<CryptoHistory, CryptoHistoryUpdateDto>().ReverseMap();
            CreateMap<CryptoPostDto, Crypto>();
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Sold ? "Sold" : "Bought"))
                .ForMember(dest => dest.Crypto, opt => opt.MapFrom(src => src.Crypto.Name));
            CreateMap<Transaction, TransactionDetailDto>()
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Sold ? "Sold" : "Bought"))
                .ForMember(dest => dest.Crypto, opt => opt.MapFrom(src => src.Crypto.Name))
                .ForMember(dest => dest.BoughtAtPrice, opt => opt.MapFrom(src => src.Crypto.ExchangeRate));
            CreateMap<TransactionDetailDto, Transaction>();
            CreateMap<Transaction, TransactionBuyDto>().ReverseMap();
            CreateMap<Transaction, TransactionSellDto>().ReverseMap();
            CreateMap<TransactionPostDto, Transaction>();
            CreateMap<WalletItemPostDto, WalletItem>();
            CreateMap<WalletItemDto, WalletItem>().ReverseMap();
            CreateMap<WalletItemPostDto, WalletItemDto>();
        }
    }
}
