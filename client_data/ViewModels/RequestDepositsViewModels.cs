
namespace client_data.ClientXmlViewsModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.UI.WebControls;

    public class RequestSoglasieViewModels
    {

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
        public string ClientId { get; set; }

        [Display(Name = "Наименование ПИФ, НПФ, АИФ")]
        [Required(AllowEmptyStrings = false)]
        public string PortfolioId { get; set; }

        [Display(Name = "ХАРАКТЕР (СПОСОБ) ПРЕДПОЛАГАЕМОГО РАСПОРЯЖЕНИЯ ИМУЩЕСТВОМ")]
        public IEnumerable<RadioButton> RubricaOut { get; set; }

        [Display(Name = "Наименование кредитной организации")]
        [Required(AllowEmptyStrings = false)]
        public string KoId { get; set; }

        [Display(Name = "Филиал")]
        public string FilialId { get; set; }

        [Display(Name = "Описание имущества")]
        public string ValueTypes { get; set; }

        [Display(Name = "Сумма вклада")]
        [Required(AllowEmptyStrings = false)]
        public decimal DepositSum { get; set; }

        [Display(Name = "Валюта вклада")]
        public string DepositCurrency { get; set; }

        [Display(Name = "Валюта расчетов")]
        public string SettlementCurrency { get; set; }

        [Display(Name = "Неснижаемый остаток")]
        public decimal BalanceMin { get; set; }

        [Display(Name = "Номер договора депозитного вклада/Генерального соглашения")]
        public string DepositDogNum { get; set; }

        [Display(Name = "Дата договора депозитного вклада/Генерального соглашения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DepositDogDate { get; set; }

        [Display(Name = "Номер дополнительного соглашения к договору")]
        public string AgreementContractNum { get; set; }

        [Display(Name = "Дата дополнительного соглашения к договору")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AgreementContractDate { get; set; }

        [Display(Name = "Дата изменения параметров договора")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ChangesDogDate { get; set; }

        [Display(Name = "Дата расторжения договора")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TerminationDogDate { get; set; }

        [Display(Name = "Тип вклада")]
        public string ContributionType { get; set; }

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
        public DateTime? TransferDateEnd { get; set; }

        [Display(Name = "Тип договора депозитного вклада")]
        public string ContributionDogType { get; set; }

        [Display(Name = "Ставка (процентов годовых)")]
        [Range(typeof(decimal), "0.0", "79228162514264337593543950335", ErrorMessage = "Значением поля Ставка (процентов годовых) должно быть положительное число.")]
        public decimal RateValue { get; set; }

        //нужен отдельный клас
        [Display(Name = "Процентные периоды")]        
        public List<PercentPeriods> PercentPeriods { get; set; }

        [Display(Name = "Периодичность выплаты процентов")]
        public string PeriodPayment { get; set; }

        [Display(Name = "Дата выплаты процентов:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PeriodsInterestDate { get; set; }

        [Display(Name = "Субординированный депозит")]
        public bool DepositSubordinated { get; set; }

        [Display(Name = "Счет, на который осуществляется возврат суммы депозита и начисленных процентов:")]
        [Required(AllowEmptyStrings = false)]
        public int AccountReturn { get; set; }

        [Display(Name = "Кредитная организация, в котором открыт счет, на который осуществляется возврат суммы депозита и начисленных процентов:")]
        public string KoAccountOpen { get; set; }

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
        public bool RequestStatus { get; set; }

        [Display(Name = "Номер Запроса")]
        public string RequestNum { get; set; }

        [Display(Name = "Дата и Время Запроса")]
        public DateTime? RequestDate { get; set; }

        [Display(Name = "Причины отказа")]
        public string RequestDescription { get; set; }


        //нужен отдельный клас
        [Display(Name = "Прилагаемые документы (при наличии):")]        
        public List<DocumentCollections> DocumentCollections { get; set; }
        
        [Display(Name = "Примечание")]
        public string CommentCD { get; set; }
                
    }

    public class DocumentCollections
    {
        [Display(Name = "Нименование файла")]
        public string DocName { get; set; }                
    }

    public class PercentPeriods
    {

        [Display(Name = "С")]
        public DateTime StartDate { get; set; }

        [Display(Name = "По")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Процент")]
        public int PercentRate { get; set; }
    }
}