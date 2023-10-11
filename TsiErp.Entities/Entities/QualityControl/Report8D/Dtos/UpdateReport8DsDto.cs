using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.Report8D.Dtos
{
    public class UpdateReport8DsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// 8D Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tedarikçi ID
        /// </summary>
        public Guid? SupplierID { get; set; }
        /// <summary>
        /// Müşteri ID
        /// </summary>
        public Guid? CustomerID { get; set; }
        /// <summary>
        /// Konu Başlığı
        /// </summary>
        public string TopicTitle { get; set; }
        /// <summary>
        /// Parça Numarası
        /// </summary>
        public string PartNumber { get; set; }
        /// <summary>
        /// Talep Açılış Tarihi
        /// </summary>
        public DateTime ClaimOpeningDate { get; set; }
        /// <summary>
        /// 8D Rapor Revizyonu
        /// </summary>
        public string Report8DRevision { get; set; }
        /// <summary>
        /// D3 Ara Raporu Tarihi
        /// </summary>
        public DateTime? DateInterimReportD3 { get; set; }
        /// <summary>
        /// D5 Ara Raporu Tarihi
        /// </summary>
        public DateTime? DateInterimReportD5 { get; set; }
        /// <summary>
        /// Son Yayınlanma Tarihi
        /// </summary>
        public DateTime? DateFinalRelease { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Teknik Resim ID
        /// </summary>
        public Guid? TechnicalDrawingID { get; set; }
        /// <summary>
        /// Çizim İndeksi
        /// </summary>
        public string DrawingIndex { get; set; }
        /// <summary>
        /// Sorun Yaşanan Tesisler
        /// </summary>
        public string ClaimingPlants { get; set; }
        /// <summary>
        /// Üretim Tesisi
        /// </summary>
        public string ProductionPlant { get; set; }
        /// <summary>
        /// Yükümlü
        /// </summary>
        public string Sponsor { get; set; }
        /// <summary>
        /// Yükümlü Görev ve Departman
        /// </summary>
        public string SponsorFunctionDepartment { get; set; }
        /// <summary>
        /// Yükümlü Telefon
        /// </summary>
        public string SponsorPhone { get; set; }
        /// <summary>
        /// Yükümlü E-Posta
        /// </summary>
        public string SponsorEMail { get; set; }
        /// <summary>
        /// Ekip Lideri
        /// </summary>
        public string TeamLeader { get; set; }
        /// <summary>
        /// Ekip Lideri Görev ve Departman
        /// </summary>
        public string TeamLeaderFunctionDepartment { get; set; }
        /// <summary>
        /// Ekip Lideri Telefon
        /// </summary>
        public string TeamLeaderPhone { get; set; }
        /// <summary>
        /// Ekip Lideri E-Posta
        /// </summary>
        public string TeamLeaderEMail { get; set; }
        /// <summary>
        /// Ekip Üyesi 1
        /// </summary>
        public string TeamMember1 { get; set; }
        /// <summary>
        /// Ekip Üyesi 1 Görev ve Departman
        /// </summary>
        public string TeamMember1FunctionDepartment { get; set; }
        /// <summary>
        /// Ekip Üyesi 1 Telefon
        /// </summary>
        public string TeamMember1Phone { get; set; }
        /// <summary>
        /// Ekip Üyesi 1 E-Posta
        /// </summary>
        public string TeamMember1EMail { get; set; }
        /// <summary>
        /// Ekip Üyesi 2
        /// </summary>
        public string TeamMember2 { get; set; }
        /// <summary>
        /// Ekip Üyesi 2 Görev ve Departman
        /// </summary>
        public string TeamMember2FunctionDepartment { get; set; }
        /// <summary>
        /// Ekip Üyesi 2 Telefon
        /// </summary>
        public string TeamMember2Phone { get; set; }
        /// <summary>
        /// Ekip Üyesi 2 E-Posta
        /// </summary>
        public string TeamMember2EMail { get; set; }
        /// <summary>
        /// Ekip Üyesi 3
        /// </summary>
        public string TeamMember3 { get; set; }
        /// <summary>
        /// Ekip Üyesi 3 Görev ve Departman
        /// </summary>
        public string TeamMember3FunctionDepartment { get; set; }
        /// <summary>
        /// Ekip Üyesi 3 Telefon
        /// </summary>
        public string TeamMember3Phone { get; set; }
        /// <summary>
        /// Ekip Üyesi 3 E-Posta
        /// </summary>
        public string TeamMember3EMail { get; set; }
        /// <summary>
        /// Hatalar Belirtiler
        /// </summary>
        public string DeviationsSymptoms { get; set; }
        /// <summary>
        /// Hatalar Sorunlar
        /// </summary>
        public string DeviationsProblems { get; set; }
        /// <summary>
        /// Teslim Edilen Miktar
        /// </summary>
        public decimal DeliveredQuantity { get; set; }
        /// <summary>
        /// Şikayet Edilen Miktar
        /// </summary>
        public decimal ClaimedQuantity { get; set; }
        /// <summary>
        /// Etkilenen Diğer Müşteri Tesisleri
        /// </summary>
        public string OtherAffectedPlants { get; set; }
        /// <summary>
        /// Şikayet Haklı mı?
        /// </summary>
        public string ComplaintJustified { get; set; }
        /// <summary>
        /// Kapsam Belirleme Tarihi
        /// </summary>
        public DateTime? ContainmentActionDate { get; set; }
        /// <summary>
        /// Tedarikçide Bloke
        /// </summary>
        public int AtSupplierBlocked { get; set; }
        /// <summary>
        /// Tedarikçide Kontrol Edilen
        /// </summary>
        public int AtSupplierChecked { get; set; }
        /// <summary>
        /// Tedarikçide Hatalı
        /// </summary>
        public int AtSupplierDefect { get; set; }
        /// <summary>
        /// Nakliyede Bloke
        /// </summary>
        public int InTransitBlocked { get; set; }
        /// <summary>
        /// Nakliyede Kontrol Edilen
        /// </summary>
        public int InTransitChecked { get; set; }
        /// <summary>
        /// Nakliyede Hatalı
        /// </summary>
        public int InTransitDefect { get; set; }
        /// <summary>
        /// Müşteri Deposunda Bloke
        /// </summary>
        public int AtCustomerBlocked { get; set; }
        /// <summary>
        /// Müşteri Deposunda Kontrol Edilen
        /// </summary>
        public int AtCustomerChecked { get; set; }
        /// <summary>
        /// Müşteri Deposunda Hatalı
        /// </summary>
        public int AtCustomerDefect { get; set; }
        /// <summary>
        /// CA1 Kapsam Belirleme Eylemi
        /// </summary>
        public string CA1ContainmentAction { get; set; }
        /// <summary>
        /// CA1 Uygulama Tarihi
        /// </summary>
        public DateTime? CA1ImplementationDate { get; set; }
        /// <summary>
        /// CA1 Sorumlu
        /// </summary>
        public string CA1Responsible { get; set; }
        /// <summary>
        /// CA1 Potansiyel Risk
        /// </summary>
        public string CA1PotentialRisk { get; set; }
        /// <summary>
        /// CA2 Kapsam Belirleme Eylemi
        /// </summary>
        public string CA2ContainmentAction { get; set; }
        /// <summary>
        /// CA2 Uygulama Tarihi
        /// </summary>
        public DateTime? CA2ImplementationDate { get; set; }
        /// <summary>
        /// CA2 Sorumlu
        /// </summary>
        public string CA2Responsible { get; set; }
        /// <summary>
        /// CA2 Potansiyel Risk
        /// </summary>
        public string CA2PotentialRisk { get; set; }
        /// <summary>
        /// CA3 Kapsam Belirleme Eylemi
        /// </summary>
        public string CA3ContainmentAction { get; set; }
        /// <summary>
        /// CA3 Uygulama Tarihi
        /// </summary>
        public DateTime? CA3ImplementationDate { get; set; }
        /// <summary>
        /// CA3 Sorumlu
        /// </summary>
        public string CA3Responsible { get; set; }
        /// <summary>
        /// CA3 Potansiyel Risk
        /// </summary>
        public string CA3PotentialRisk { get; set; }
        /// <summary>
        /// Hata Sıklığı
        /// </summary>
        public string FailureOccurance { get; set; }
        /// <summary>
        /// RO1 Meydana Gelme Sebebi
        /// </summary>
        public string RO1OccuranceReason { get; set; }
        /// <summary>
        /// RO1 Paylaşım
        /// </summary>
        public string RO1Share { get; set; }
        /// <summary>
        /// RO1 Analiz Yöntemi
        /// </summary>
        public string RO1AnalysisMethod { get; set; }
        /// <summary>
        /// RO2 Meydana Gelme Sebebi
        /// </summary>
        public string RO2OccuranceReason { get; set; }
        /// <summary>
        /// RO2 Paylaşım
        /// </summary>
        public string RO2Share { get; set; }
        /// <summary>
        /// RO2 Analiz Yöntemi
        /// </summary>
        public string RO2AnalysisMethod { get; set; }
        /// <summary>
        /// RN1 Tespit Edilmeme Sebebi
        /// </summary>
        public string RN1NonDetectionReason { get; set; }
        /// <summary>
        /// RN1 Paylaşım
        /// </summary>
        public string RN1Share { get; set; }
        /// <summary>
        /// RN1 Analiz Yöntemi
        /// </summary>
        public string RN1AnalysisMethod { get; set; }
        /// <summary>
        /// RN2 Tespit Edilmeme Sebebi
        /// </summary>
        public string RN2NonDetectionReason { get; set; }
        /// <summary>
        /// RN2 Paylaşım
        /// </summary>
        public string RN2Share { get; set; }
        /// <summary>
        /// RN2 Analiz Yöntemi
        /// </summary>
        public string RN2AnalysisMethod { get; set; }
        /// <summary>
        /// PA1 Kök Sebep
        /// </summary>
        public string PA1RootCause { get; set; }
        /// <summary>
        /// PA1 Potansiyel Düzeltici Faaliyet
        /// </summary>
        public string PA1PotentialCorrectiveAction { get; set; }
        /// <summary>
        /// PA1 Uygulanacak
        /// </summary>
        public string PA1ToBeImplemented { get; set; }
        /// <summary>
        /// PA1 Sorumlu
        /// </summary>
        public string PA1Responsible { get; set; }
        /// <summary>
        /// PA1 Planlanan Uygulama Tarihi
        /// </summary>
        public DateTime? PA1PlannedImplementationDate { get; set; }
        /// <summary>
        /// PA2 Kök Sebep
        /// </summary>
        public string PA2RootCause { get; set; }
        /// <summary>
        /// PA2 Potansiyel Düzeltici Faaliyet
        /// </summary>
        public string PA2PotentialCorrectiveAction { get; set; }
        /// <summary>
        /// PA2 Uygulanacak
        /// </summary>
        public string PA2ToBeImplemented { get; set; }
        /// <summary>
        /// PA2 Sorumlu
        /// </summary>
        public string PA2Responsible { get; set; }
        /// <summary>
        /// PA2 Planlanan Uygulama Tarihi
        /// </summary>
        public DateTime? PA2PlannedImplementationDate { get; set; }
        /// <summary>
        /// PA3 Kök Sebep
        /// </summary>
        public string PA3RootCause { get; set; }
        /// <summary>
        /// PA3 Potansiyel Düzeltici Faaliyet
        /// </summary>
        public string PA3PotentialCorrectiveAction { get; set; }
        /// <summary>
        /// PA3 Uygulanacak
        /// </summary>
        public string PA3ToBeImplemented { get; set; }
        /// <summary>
        /// PA3 Sorumlu
        /// </summary>
        public string PA3Responsible { get; set; }
        /// <summary>
        /// PA3 Planlanan Uygulama Tarihi
        /// </summary>
        public DateTime? PA3PlannedImplementationDate { get; set; }
        /// <summary>
        /// PA4 Kök Sebep
        /// </summary>
        public string PA4RootCause { get; set; }
        /// <summary>
        /// PA4 Potansiyel Düzeltici Faaliyet
        /// </summary>
        public string PA4PotentialCorrectiveAction { get; set; }
        /// <summary>
        /// PA4 Uygulanacak
        /// </summary>
        public string PA4ToBeImplemented { get; set; }
        /// <summary>
        /// PA4 Sorumlu
        /// </summary>
        public string PA4Responsible { get; set; }
        /// <summary>
        /// PA4 Planlanan Uygulama Tarihi
        /// </summary>
        public DateTime? PA4PlannedImplementationDate { get; set; }
        /// <summary>
        /// PA5 Kök Sebep
        /// </summary>
        public string PA5RootCause { get; set; }
        /// <summary>
        /// PA5 Potansiyel Düzeltici Faaliyet
        /// </summary>
        public string PA5PotentialCorrectiveAction { get; set; }
        /// <summary>
        /// PA5 Uygulanacak
        /// </summary>
        public string PA5ToBeImplemented { get; set; }
        /// <summary>
        /// PA5 Sorumlu
        /// </summary>
        public string PA5Responsible { get; set; }
        /// <summary>
        /// PA5 Planlanan Uygulama Tarihi
        /// </summary>
        public DateTime? PA5PlannedImplementationDate { get; set; }
        /// <summary>
        /// IA1 Kök Sebep
        /// </summary>
        public string IA1RootCause { get; set; }
        /// <summary>
        /// IA1 Düzeltici Faaliyet
        /// </summary>
        public string IA1CorrectiveAction { get; set; }
        /// <summary>
        /// IA1 Uygulama Tarihi
        /// </summary>
        public DateTime? IA1ImplementationDate { get; set; }
        /// <summary>
        /// IA1 İtibaren Geçerli Tarihi
        /// </summary>
        public DateTime? IA1EffectiveFromDate { get; set; }
        /// <summary>
        /// IA1 Doğrulanma Tarihi
        /// </summary>
        public DateTime? IA1ValidatedDate { get; set; }
        /// <summary>
        /// IA1 Kanıt Eklendi
        /// </summary>
        public string IA1ProofAttached { get; set; }
        /// <summary>
        /// IA2 Kök Sebep
        /// </summary>
        public string IA2RootCause { get; set; }
        /// <summary>
        /// IA2 Düzeltici Faaliyet
        /// </summary>
        public string IA2CorrectiveAction { get; set; }
        /// <summary>
        /// IA2 Uygulama Tarihi
        /// </summary>
        public DateTime? IA2ImplementationDate { get; set; }
        /// <summary>
        /// IA2 İtibaren Geçerli Tarihi
        /// </summary>
        public DateTime? IA2EffectiveFromDate { get; set; }
        /// <summary>
        /// IA2 Doğrulanma Tarihi
        /// </summary>
        public DateTime? IA2ValidatedDate { get; set; }
        /// <summary>
        /// IA2 Kanıt Eklendi
        /// </summary>
        public string IA2ProofAttached { get; set; }
        /// <summary>
        /// IA3 Kök Sebep
        /// </summary>
        public string IA3RootCause { get; set; }
        /// <summary>
        /// IA3 Düzeltici Faaliyet
        /// </summary>
        public string IA3CorrectiveAction { get; set; }
        /// <summary>
        /// IA3 Uygulama Tarihi
        /// </summary>
        public DateTime? IA3ImplementationDate { get; set; }
        /// <summary>
        /// IA3 İtibaren Geçerli Tarihi
        /// </summary>
        public DateTime? IA3EffectiveFromDate { get; set; }
        /// <summary>
        /// IA3 Doğrulanma Tarihi
        /// </summary>
        public DateTime? IA3ValidatedDate { get; set; }
        /// <summary>
        /// IA3 Kanıt Eklendi
        /// </summary>
        public string IA3ProofAttached { get; set; }
        /// <summary>
        /// IA4 Kök Sebep
        /// </summary>
        public string IA4RootCause { get; set; }
        /// <summary>
        /// IA4 Düzeltici Faaliyet
        /// </summary>
        public string IA4CorrectiveAction { get; set; }
        /// <summary>
        /// IA4 Uygulama Tarihi
        /// </summary>
        public DateTime? IA4ImplementationDate { get; set; }
        /// <summary>
        /// IA4 İtibaren Geçerli Tarihi
        /// </summary>
        public DateTime? IA4EffectiveFromDate { get; set; }
        /// <summary>
        /// IA4 Doğrulanma Tarihi
        /// </summary>
        public DateTime? IA4ValidatedDate { get; set; }
        /// <summary>
        /// IA4 Kanıt Eklendi
        /// </summary>
        public string IA4ProofAttached { get; set; }
        /// <summary>
        /// IA5 Kök Sebep
        /// </summary>
        public string IA5RootCause { get; set; }
        /// <summary>
        /// IA5 Düzeltici Faaliyet
        /// </summary>
        public string IA5CorrectiveAction { get; set; }
        /// <summary>
        /// IA5 Uygulama Tarihi
        /// </summary>
        public DateTime? IA5ImplementationDate { get; set; }
        /// <summary>
        /// IA5 İtibaren Geçerli Tarihi
        /// </summary>
        public DateTime? IA5EffectiveFromDate { get; set; }
        /// <summary>
        /// IA5 Doğrulanma Tarihi
        /// </summary>
        public DateTime? IA5ValidatedDate { get; set; }
        /// <summary>
        /// IA5 Kanıt Eklendi
        /// </summary>
        public string IA5ProofAttached { get; set; }
        /// <summary>
        /// CA1 Kapsam Belirleme Eylemi D6
        /// </summary>
        public string CA1ContainmentActionD6 { get; set; }
        /// <summary>
        /// CA1 Kaldırılma Tarihi D6
        /// </summary>
        public DateTime? CA1RemovalDateD6 { get; set; }
        /// <summary>
        /// CA1 Sorumlu D6
        /// </summary>
        public string CA1ResponsibleD6 { get; set; }
        /// <summary>
        /// CA2 Kapsam Belirleme Eylemi D6
        /// </summary>
        public string CA2ContainmentActionD6 { get; set; }
        /// <summary>
        /// CA2 Kaldırılma Tarihi D6
        /// </summary>
        public DateTime? CA2RemovalDateD6 { get; set; }
        /// <summary>
        /// CA2 Sorumlu D6
        /// </summary>
        public string CA2ResponsibleD6 { get; set; }
        /// <summary>
        /// CA3 Kapsam Belirleme Eylemi D6
        /// </summary>
        public string CA3ContainmentActionD6 { get; set; }
        /// <summary>
        /// CA3 Kaldırılma Tarihi D6
        /// </summary>
        public DateTime? CA3RemovalDateD6 { get; set; }
        /// <summary>
        /// CA3 Sorumlu D6
        /// </summary>
        public string CA3ResponsibleD6 { get; set; }
        /// <summary>
        /// DFMEA Revizyon Bağlantılı
        /// </summary>
        public string DFMEARevisionRelevant { get; set; }
        /// <summary>
        /// DFMEA Revizyon Belge Numarası
        /// </summary>
        public string DFMEARevisionDocumentNumber { get; set; }
        /// <summary>
        /// DFMEA Revizyon Versiyon
        /// </summary>
        public string DFMEARevisionVersion { get; set; }
        /// <summary>
        /// DFMEA Revizyon Sorumlu
        /// </summary>
        public string DFMEARevisionResponsible { get; set; }
        /// <summary>
        /// DFMEA Revizyon Tamamlanma Tarihi
        /// </summary>
        public DateTime? DFMEARevisionCompletionDate { get; set; }
        /// <summary>
        /// DFMEA Revizyon Kanıt Eklendi
        /// </summary>
        public string DFMEARevisionProofAttached { get; set; }
        /// <summary>
        /// PFMEA Revizyon Bağlantılı
        /// </summary>
        public string PFMEARevisionRelevant { get; set; }
        /// <summary>
        /// PFMEA Revizyon Belge Numarası
        /// </summary>
        public string PFMEARevisionDocumentNumber { get; set; }
        /// <summary>
        /// PFMEA Revizyon Versiyon
        /// </summary>
        public string PFMEARevisionVersion { get; set; }
        /// <summary>
        /// PFMEA Revizyon Sorumlu
        /// </summary>
        public string PFMEARevisionResponsible { get; set; }
        /// <summary>
        /// PFMEA Revizyon Tamamlanma Tarihi
        /// </summary>
        public DateTime? PFMEARevisionCompletionDate { get; set; }
        /// <summary>
        /// PFMEA Revizyon Kanıt Eklendi
        /// </summary>
        public string PFMEARevisionProofAttached { get; set; }
        /// <summary>
        /// Kontrol Planı Revizyon Bağlantılı
        /// </summary>
        public string ControlPlanRevisionRelevant { get; set; }
        /// <summary>
        /// Kontrol Planı Revizyon Belge Numarası
        /// </summary>
        public string ControlPlanRevisionDocumentNumber { get; set; }
        /// <summary>
        /// Kontrol Planı Revizyon Versiyon
        /// </summary>
        public string ControlPlanRevisionVersion { get; set; }
        /// <summary>
        /// Kontrol Planı Revizyon Sorumlu
        /// </summary>
        public string ControlPlanRevisionResponsible { get; set; }
        /// <summary>
        /// Kontrol Planı Revizyon Tamamlanma Tarihi
        /// </summary>
        public DateTime? ControlPlanRevisionCompletionDate { get; set; }
        /// <summary>
        /// Kontrol Planı Revizyon Kanıt Eklendi
        /// </summary>
        public string ControlPlanRevisionProofAttached { get; set; }
        /// <summary>
        /// Revizyon 1 Eylem
        /// </summary>
        public string Revision1Action { get; set; }
        /// <summary>
        /// Revizyon 1 Bağlantılı
        /// </summary>
        public string Revision1Relevant { get; set; }
        /// <summary>
        /// Revizyon 1 Belge Numarası
        /// </summary>
        public string Revision1DocumentNumber { get; set; }
        /// <summary>
        /// Revizyon 1 Versiyon
        /// </summary>
        public string Revision1Version { get; set; }
        /// <summary>
        /// Revizyon 1 Sorumlu
        /// </summary>
        public string Revision1Responsible { get; set; }
        /// <summary>
        /// Revizyon 1 Tamamlanma Tarihi
        /// </summary>
        public DateTime? Revision1CompletionDate { get; set; }
        /// <summary>
        /// Revizyon 1 Kanıt Eklendi
        /// </summary>
        public string Revision1ProofAttached { get; set; }
        /// <summary>
        /// Revizyon 2 Eylem
        /// </summary>
        public string Revision2Action { get; set; }
        /// <summary>
        /// Revizyon 2 Bağlantılı
        /// </summary>
        public string Revision2Relevant { get; set; }
        /// <summary>
        /// Revizyon 2 Belge Numarası
        /// </summary>
        public string Revision2DocumentNumber { get; set; }
        /// <summary>
        /// Revizyon 2 Versiyon
        /// </summary>
        public string Revision2Version { get; set; }
        /// <summary>
        /// Revizyon 2 Sorumlu
        /// </summary>
        public string Revision2Responsible { get; set; }
        /// <summary>
        /// Revizyon 2 Tamamlanma Tarihi
        /// </summary>
        public DateTime? Revision2CompletionDate { get; set; }
        /// <summary>
        /// Revizyon 2 Kanıt Eklendi
        /// </summary>
        public string Revision2ProofAttached { get; set; }
        /// <summary>
        /// Revizyon 3 Eylem
        /// </summary>
        public string Revision3Action { get; set; }
        /// <summary>
        /// Revizyon 3 Bağlantılı
        /// </summary>
        public string Revision3Relevant { get; set; }
        /// <summary>
        /// Revizyon 3 Belge Numarası
        /// </summary>
        public string Revision3DocumentNumber { get; set; }
        /// <summary>
        /// Revizyon 3 Versiyon
        /// </summary>
        public string Revision3Version { get; set; }
        /// <summary>
        /// Revizyon 3 Sorumlu
        /// </summary>
        public string Revision3Responsible { get; set; }
        /// <summary>
        /// Revizyon 3 Tamamlanma Tarihi
        /// </summary>
        public DateTime? Revision3CompletionDate { get; set; }
        /// <summary>
        /// Revizyon 3 Kanıt Eklendi
        /// </summary>
        public string Revision3ProofAttached { get; set; }
        /// <summary>
        /// Öğrenilen Dersler Bağlantılı
        /// </summary>
        public string LessonsLearnedRelevant { get; set; }
        /// <summary>
        /// Öğrenilen Dersler Sorumlu
        /// </summary>
        public string LessonsLearnedResponsible { get; set; }
        /// <summary>
        /// Öğrenilen Dersler Görev ve Departman
        /// </summary>
        public string LessonsLearnedFunctionDepartment { get; set; }
        /// <summary>
        /// Öğrenilen Dersler Tarihi
        /// </summary>
        public DateTime? LessonsLearnedDate { get; set; }
        /// <summary>
        /// Öğrenilen Dersler Kanıt Eklendi
        /// </summary>
        public string LessonsLearnedProofAttached { get; set; }
        /// <summary>
        /// Ekip Lideri D8
        /// </summary>
        public string TeamLeaderD8 { get; set; }
        /// <summary>
        /// Ekip Lideri Tarih D8
        /// </summary>
        public DateTime? TeamLeaderDateD8 { get; set; }
        /// <summary>
        /// Yükümlü D8
        /// </summary>
        public string SponsorD8 { get; set; }
        /// <summary>
        /// Yükümlü Tarih D8
        /// </summary>
        public DateTime? SponsorDateD8 { get; set; }
        /// <summary>
        /// 8D Kabul Edildi
        /// </summary>
        public string Report8DAccepted { get; set; }
        /// <summary>
        /// Güncelleme Gereken Tarih
        /// </summary>
        public DateTime? UpdateRequiredUntilDate { get; set; }
        /// <summary>
        /// Müşteri 8D Kapatma Adı
        /// </summary>
        public string Customer8DClosureName { get; set; }
        /// <summary>
        /// Müşteri 8D Kapatma Görev ve Departman
        /// </summary>
        public string Customer8DClosureFunctionDepartment { get; set; }
        /// <summary>
        /// Müşteri 8D Kapatma Tarih
        /// </summary>
        public DateTime? Customer8DClosureDate { get; set; }
    }
}
