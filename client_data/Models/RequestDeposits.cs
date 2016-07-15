
namespace client_data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("RequestDeposit", IsNullable = true)]
    public class RequestDeposits
    {
        public RequestDeposits()
        {
            OutDate = DateTime.Now.Date;
            TransferDateEnd = DateTime.Now.Date;
            Operation = 3;
            ChildCat = 667;
            CommentCD = "Не является выданным согласием СД";
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [XmlElement("Guid")]
        public Guid Id { get; set; }
                
        [Display(Name = "Исходящий №:")]
        [Required(AllowEmptyStrings = false)]
        public string OutNumber { get; set; }

        [Display(Name = "Дата исходящего")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime OutDate { get; set; }

        [Display(Name = "Номер и дата Правил ДУ договора доверительного управления НПФ, АИФ")]
        public string DuNumDate { get; set; }

        [Display(Name = "Управляющая компания")]
        [Required(AllowEmptyStrings = false)]
        public int ClientId { get; set; }

        [Display(Name = "Наименование ПИФ, НПФ, АИФ")]
        [Required(AllowEmptyStrings = false)]
        public int PortfolioId { get; set; }        

        [Display(Name = "ХАРАКТЕР (СПОСОБ) ПРЕДПОЛАГАЕМОГО РАСПОРЯЖЕНИЯ ИМУЩЕСТВОМ")]
        public int RubricaOut { get; set; }
                
        [Display(Name = "Наименование кредитной организации")]
        [Required(AllowEmptyStrings = false)]
        public int KoId { get; set; }

        [Display(Name = "Филиал")]
        public int? FilialId { get; set; }

        [Display(Name = "Описание имущества")]
        public int ValueTypes { get; set; }

        [Display(Name = "Сумма вклада")]
        [Required(AllowEmptyStrings = false)]
        [Range(typeof(decimal), "0.0", "79228162514264337593543950335", ErrorMessage = "Значением поля Сумма вклада должно быть положительное число.")]
        public decimal DepositSum { get; set; }

        [Display(Name = "Валюта вклада")]
        public int DepositCurrency { get; set; }

        [Display(Name = "Валюта расчетов")]
        public int SettlementCurrency { get; set; }

        [Display(Name = "Неснижаемый остаток")]
        [Range(typeof(decimal), "0.0", "79228162514264337593543950335", ErrorMessage = "Значением поля Неснижаемый остаток должно быть положительное число.")]
        public decimal BalanceMin { get; set; }

        [Display(Name = "Номер договора депозитного вклада/Генерального соглашения")]
        public string DepositDogNum { get; set; }

        [Display(Name = "Дата договора депозитного вклада/Генерального соглашения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DepositDogDate { get; set; }
        
        [Display(Name = "Тип вклада")]
        public int ContributionType { get; set; }

        //[Display(Name = "Дата завершения договора депозитного вклада")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? DepositDogDateEnd { get; set; }

        [Display(Name = "Дата завершения договора депозитного вклада не превышает 6 мес. с даты окончания срока действия Правил ДУ")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public string DepositDogDateEnd { get; set; }

        [Display(Name = "Депозитный счет")]
        [StringLength(20, MinimumLength = 20, ErrorMessage = "Поле должно содержать 20 знаков")]
        public string DepositAccount { get; set; }

        [Display(Name = "Крайняя (последняя) дата перечисления денежных средств в депозит")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TransferDateEnd { get; set; }

        [Display(Name = "Тип договора депозитного вклада")]
        public int ContributionDogType { get; set; }
                
        [Display(Name = "Ставка (процентов годовых)")]
        [Range(typeof(decimal), "0.0", "79228162514264337593543950335", ErrorMessage = "Значением поля Ставка (процентов годовых) должно быть положительное число.")]
        public decimal RateValue { get; set; }
        
        //нужен отдельный клас
        [Display(Name = "Процентные периоды")]
        [XmlArray("PercentPeriods")]
        public List<PercentPeriods> PercentPeriods { get; set; }

        [Display(Name = "Порядок начисления и выплаты процентов")]
        public int PercentOutType { get; set; }

        [Display(Name = "Периодичность выплаты процентов")]
        public int PeriodPayment { get; set; }
        
        [Display(Name = "Дата выплаты процентов:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PeriodsInterestDate { get; set; }

        [Display(Name = "Субординированный депозит")]
        public bool DepositSubordinated { get; set; }

        [Display(Name = "Ставка (процентов годовых) при досрочном расторжении")]
        [Range(typeof(decimal), "0.0", "79228162514264337593543950335", ErrorMessage = "Значением поля Ставка (процентов годовых) при досрочном расторжении должно быть положительное число.")]
        public decimal RateValueNercent { get; set; }

        [Display(Name = "Счет, на который осуществляется возврат суммы депозита и начисленных процентов:")]
        [Required(AllowEmptyStrings = false)]
        public int AccountReturn { get; set; }

        [Display(Name = "Кредитная организация, в котором открыт счет, на который осуществляется возврат суммы депозита и начисленных процентов:")]
        public int? KoAccountOpen { get; set; }
                
        [Display(Name = "Наличие в Договоре условия о том, что в случае досрочного расторжения договора в связи с тем, что кредитная организация перестала удовлетворять требованиям, предусмотренным пунктом 2.1  Положения №451-П от 25.12.2014г., такая кредитная организация по требованию возвращает сумму депозита (остатка на счете) и проценты по нему, начисленные исходя из процентной ставки, определенной договором.")]
        public bool ExistenceContractConditions { get; set; }

        //Отсутствие в Договоре условия о том, что Банк вправе списывать с депозитного счета
        //в безакцептном порядке денежные средства в оплату задолженности Клиента по иным договорам
        [Display(Name = "Отсутствие в Договоре условия о том, что Банк вправе списывать с депозитного счета в безакцептном порядке денежные средства в оплату задолженности Клиента по иным договорам")]
        public bool NoExistenceContractConditions { get; set; }
        
        [Display(Name = "ФИО Уполномоченного лица")]
        [Required(AllowEmptyStrings = false)]
        public string AuthorizedPersonFIO { get; set; }

        [Display(Name = "Должность Уполномоченного лица")]
        [Required(AllowEmptyStrings = false)]
        public string AuthorizedPersonPost { get; set; }

        //0 - Согласие
        //1 - Отказ
        [Display(Name = "Статус Запроса")]
        public int RequestStatus { get; set; }

        [Display(Name = "Номер Запроса")]
        public string RequestNum { get; set; }

        [Display(Name = "Дата и Время Запроса")]
        public DateTime? RequestDate { get; set; }

        [Display(Name = "Причины отказа")]
        public string RequestDescription { get; set; }


        public Guid? RequestId { get; set; }


        [Display(Name = "Примечание")]
        public string CommentCD { get; set; }


        //Статика дя FANSY
        public int ChildCat { get; set; }        
        public int Operation { get; set; }
        

        [XmlIgnore]
        public string AppUserId { get; set; }

        [Display(Name = "Статус")]
        [XmlIgnore]
        public int StatusObrobotki { get; set; }
    }
}