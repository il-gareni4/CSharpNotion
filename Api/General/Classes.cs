using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharpNotion.Api.General
{
    public class Pointer
    {
        public string Id { get; set; }
        public string Table { get; set; }

        public Pointer(string id, string table)
        { Id = id; Table = table; }
    }

    public sealed class BlockFormat
    {
        [JsonPropertyName("copied_from_pointer")]
        public Pointer? CopiedFromPointer { get; set; }

        [JsonPropertyName("page_icon")]
        public string? PageIcon { get; set; }

        [JsonPropertyName("page_cover")]
        public string? PageCover { get; set; }

        [JsonPropertyName("page_cover_position")]
        public float? PageCoverPosition { get; set; }

        [JsonPropertyName("block_color")]
        public string? BlockColor { get; set; }

        [JsonPropertyName("display_source")]
        public string? DisplaySource { get; set; }

        [JsonPropertyName("block_width")]
        public int? BlockWidth { get; set; }

        [JsonPropertyName("block_full_width")]
        public bool? BlockFullWidth { get; set; }

        [JsonPropertyName("block_page_width")]
        public bool? BlockPageWidth { get; set; }

        [JsonPropertyName("block_aspect_ratio")]
        public float? BlockAspectRatio { get; set; }

        [JsonPropertyName("block_preserve_scale")]
        public bool? BlockPreserveScale { get; set; }

        // TODO: make enum
        [JsonPropertyName("quote_size")]
        public string? QuoteSize { get; set; }

        [JsonPropertyName("code_wrap")]
        public bool? CodeWrap { get; set; }

        [JsonPropertyName("alias_pointer")]
        public Pointer? AliasPointer { get; set; }

        public bool? Toggleable { get; set; }

        [JsonPropertyName("transclusion_reference_pointer")]
        public Pointer? TransclusionReferencePointer { get; set; }

        [JsonPropertyName("collection_pointer")]
        public Pointer? CollectionPointer { get; set; }
    }

    internal class RecordMap
    {
        //[JsonPropertyName("space_view")]
        //public Dictionary<string, object>? SpaceView { get; set; }

        public Dictionary<string, RecordMapRoleValue<RecordMapBlockValue>>? Block { get; set; }

        public Dictionary<string, RecordMapRoleValue<RecordMapCollectionValue>>? Collection { get; set; }

        [JsonPropertyName("user_settings")]
        public Dictionary<string, RecordMapRoleValue<RecordMapUserSettings>>? UserSettings { get; set; }
    }

    internal class RecordMapRoleValue<T>
    {
        public string? Role { get; set; }
        public T? Value { get; set; }
    }

    public class RecordMapCollectionValue
    {
        public string? Id { get; set; }
        public int Version { get; set; }
        public JsonElement[][]? Name { get; set; }
        public Dictionary<string, CollectionValueSchemaElement>? Schema { get; set; }

        [JsonPropertyName("parent_id")]
        public string? ParentId { get; set; }

        [JsonPropertyName("parent_table")]
        public string? ParentTable { get; set; }

        public bool Alive { get; set; }
        public bool Migrated { get; set; }

        [JsonPropertyName("space_id")]
        public string? SpaceId { get; set; }
    }

    public class CollectionValueSchemaElement
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        internal CollectionSelectOption[]? Options { get; set; }

        [JsonPropertyName("number_format")]
        public string? NumberFormat { get; set; }

        [JsonPropertyName("date_format")]
        public string? DateFormat { get; set; }

        [JsonPropertyName("time_format")]
        public string? TimeFormat { get; set; }

        public string? Property { get; set; }
        public string? CollectionId { get; set; }

        [JsonPropertyName("collection_pointer")]
        public Pointer? CollectionPointer { get; set; }

        public string? Aggregation { get; set; }

        [JsonPropertyName("target_property")]
        public string? TargetProperty { get; set; }

        [JsonPropertyName("relation_property")]
        public string? RelationProperty { get; set; }

        [JsonPropertyName("target_property_type")]
        public string? TargetPropertyType { get; set; }
    }

    internal class CollectionSelectOption
    {
        public string? Id { get; set; }
        public string? Color { get; set; }
        public string? Value { get; set; }
    }

    public class RecordMapBlockValue
    {
        public string? Id { get; set; }
        public int Version { get; set; }
        public string? Type { get; set; }
        public string[]? Content { get; set; }
        public RecordMapBlockPermission[]? Permissions { get; set; }

        [JsonPropertyName("created_time")]
        public long CreatedTime { get; set; }

        [JsonPropertyName("last_edited_time")]
        public long LastEditedTime { get; set; }

        [JsonPropertyName("parent_id")]
        public string? ParentId { get; set; }

        [JsonPropertyName("parent_table")]
        public string? ParentTable { get; set; }

        public bool Alive { get; set; }

        [JsonPropertyName("created_by_table")]
        public string? CreatedByTable { get; set; }

        [JsonPropertyName("created_by_id")]
        public string? CreatedById { get; set; }

        [JsonPropertyName("last_edited_by_table")]
        public string? LastEditedByTable { get; set; }

        [JsonPropertyName("last_edited_by_id")]
        public string? LastEditedById { get; set; }

        [JsonPropertyName("space_id")]
        public string? SpaceId { get; set; }

        public BlockFormat? Format { get; set; }
        public string[]? Discussions { get; set; }

        [JsonPropertyName("file_ids")]
        public string[]? FileIds { get; set; }

        [JsonPropertyName("view_ids")]
        public string[]? ViewIds { get; set; }

        [JsonPropertyName("collection_id")]
        public string? CollectionId { get; set; }

        public Dictionary<string, JsonElement[][]>? Properties { get; set; }
    }

    public class RecordMapBlockPermission
    {
        public string? Role { get; set; }
        public string? Type { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }

    public class BlockDateInformation
    {
        public string? Type { get; set; }

        [JsonPropertyName("end_date")]
        public string? EndDate { get; set; }

        [JsonPropertyName("end_time")]
        public string? EndTime { get; set; }

        public BlockDateInformationReminder? Reminder { get; set; }

        [JsonPropertyName("time_zone")]
        public string? TimeZone { get; set; }

        [JsonPropertyName("start_date")]
        public string? StartDate { get; set; }

        [JsonPropertyName("start_time")]
        public string? StartTime { get; set; }
    }

    public class BlockDateInformationReminder
    {
        public string? Time { get; set; }
        public string? Unit { get; set; }
        public int Value { get; set; }
    }

    internal class RecordMapUserSettings
    {
        public string? Id { get; set; }
        public UserSettings? Settings { get; set; }
        public int Version { get; set; }
    }

    internal class UserSettings
    {
        [JsonPropertyName("cookie_consent")]
        public CookieConsent? CookieConsent { get; set; }

        public string? Locale { get; set; }
        public string? Persona { get; set; }

        [JsonPropertyName("preferred_locale")]
        public string? PreferredLocale { get; set; }

        [JsonPropertyName("preferred_locale_origin")]
        public string? PreferredLocaleOrigin { get; set; }

        [JsonPropertyName("seen_database_group_intro")]
        public bool SeenDatabaseGroupIntro { get; set; }

        [JsonPropertyName("seen_file_attachment_intro")]
        public bool SeenFileAttachmentIntro { get; set; }

        [JsonPropertyName("seen_views_intro_modal")]
        public bool SeenViewsIntroModal { get; set; }

        [JsonPropertyName("show_app_download")]
        public bool ShowAppDownload { get; set; }

        [JsonPropertyName("signup_time")]
        public long SignupTime { get; set; }

        public string? Source { get; set; }

        [JsonPropertyName("start_day_of_week")]
        public int StartDayOfWeek { get; set; }

        [JsonPropertyName("time_zone")]
        public string? TimeZone { get; set; }

        public string? Type { get; set; }

        [JsonPropertyName("used_android_app")]
        public bool UsedAndroidApp { get; set; }

        [JsonPropertyName("used_desktop_web_app")]
        public bool UsedDesktopWebApp { get; set; }

        [JsonPropertyName("used_windows_app")]
        public bool UsedWindowsApp { get; set; }
    }

    public class CookieConsent
    {
        public string? Id { get; set; }
        public CookieConsentPermission? Permission { get; set; }
    }

    public class CookieConsentPermission
    {
        public bool Necessary { get; set; }
        public bool Performance { get; set; }
        public bool Preference { get; set; }
        public bool Targeting { get; set; }
    }
}