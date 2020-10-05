//#if ENABLE_PLAYFABADMIN_API
using PlayFab.AdminModels;

namespace PlayFab.Events
{
    public partial class PlayFabEvents
    {
        public event PlayFabRequestEvent<AbortTaskInstanceRequest> OnAdminAbortTaskInstanceRequestEvent;

        public event PlayFabResultEvent<EmptyResponse> OnAdminAbortTaskInstanceResultEvent;

        public event PlayFabRequestEvent<AddLocalizedNewsRequest> OnAdminAddLocalizedNewsRequestEvent;

        public event PlayFabResultEvent<AddLocalizedNewsResult> OnAdminAddLocalizedNewsResultEvent;

        public event PlayFabRequestEvent<AddNewsRequest> OnAdminAddNewsRequestEvent;

        public event PlayFabResultEvent<AddNewsResult> OnAdminAddNewsResultEvent;

        public event PlayFabRequestEvent<AddPlayerTagRequest> OnAdminAddPlayerTagRequestEvent;

        public event PlayFabResultEvent<AddPlayerTagResult> OnAdminAddPlayerTagResultEvent;

        public event PlayFabRequestEvent<AddServerBuildRequest> OnAdminAddServerBuildRequestEvent;

        public event PlayFabResultEvent<AddServerBuildResult> OnAdminAddServerBuildResultEvent;

        public event PlayFabRequestEvent<AddUserVirtualCurrencyRequest> OnAdminAddUserVirtualCurrencyRequestEvent;

        public event PlayFabResultEvent<ModifyUserVirtualCurrencyResult> OnAdminAddUserVirtualCurrencyResultEvent;

        public event PlayFabRequestEvent<AddVirtualCurrencyTypesRequest> OnAdminAddVirtualCurrencyTypesRequestEvent;

        public event PlayFabResultEvent<BlankResult> OnAdminAddVirtualCurrencyTypesResultEvent;

        public event PlayFabRequestEvent<BanUsersRequest> OnAdminBanUsersRequestEvent;

        public event PlayFabResultEvent<BanUsersResult> OnAdminBanUsersResultEvent;

        public event PlayFabRequestEvent<CheckLimitedEditionItemAvailabilityRequest> OnAdminCheckLimitedEditionItemAvailabilityRequestEvent;

        public event PlayFabResultEvent<CheckLimitedEditionItemAvailabilityResult> OnAdminCheckLimitedEditionItemAvailabilityResultEvent;

        public event PlayFabRequestEvent<CreateActionsOnPlayerSegmentTaskRequest> OnAdminCreateActionsOnPlayersInSegmentTaskRequestEvent;

        public event PlayFabResultEvent<CreateTaskResult> OnAdminCreateActionsOnPlayersInSegmentTaskResultEvent;

        public event PlayFabRequestEvent<CreateCloudScriptTaskRequest> OnAdminCreateCloudScriptTaskRequestEvent;

        public event PlayFabResultEvent<CreateTaskResult> OnAdminCreateCloudScriptTaskResultEvent;

        public event PlayFabRequestEvent<CreateInsightsScheduledScalingTaskRequest> OnAdminCreateInsightsScheduledScalingTaskRequestEvent;

        public event PlayFabResultEvent<CreateTaskResult> OnAdminCreateInsightsScheduledScalingTaskResultEvent;

        public event PlayFabRequestEvent<CreateOpenIdConnectionRequest> OnAdminCreateOpenIdConnectionRequestEvent;

        public event PlayFabResultEvent<EmptyResponse> OnAdminCreateOpenIdConnectionResultEvent;

        public event PlayFabRequestEvent<CreatePlayerSharedSecretRequest> OnAdminCreatePlayerSharedSecretRequestEvent;

        public event PlayFabResultEvent<CreatePlayerSharedSecretResult> OnAdminCreatePlayerSharedSecretResultEvent;

        public event PlayFabRequestEvent<CreatePlayerStatisticDefinitionRequest> OnAdminCreatePlayerStatisticDefinitionRequestEvent;

        public event PlayFabResultEvent<CreatePlayerStatisticDefinitionResult> OnAdminCreatePlayerStatisticDefinitionResultEvent;

        public event PlayFabRequestEvent<DeleteContentRequest> OnAdminDeleteContentRequestEvent;

        public event PlayFabResultEvent<BlankResult> OnAdminDeleteContentResultEvent;

        public event PlayFabRequestEvent<DeleteMasterPlayerAccountRequest> OnAdminDeleteMasterPlayerAccountRequestEvent;

        public event PlayFabResultEvent<DeleteMasterPlayerAccountResult> OnAdminDeleteMasterPlayerAccountResultEvent;

        public event PlayFabRequestEvent<DeleteOpenIdConnectionRequest> OnAdminDeleteOpenIdConnectionRequestEvent;

        public event PlayFabResultEvent<EmptyResponse> OnAdminDeleteOpenIdConnectionResultEvent;

        public event PlayFabRequestEvent<DeletePlayerRequest> OnAdminDeletePlayerRequestEvent;

        public event PlayFabResultEvent<DeletePlayerResult> OnAdminDeletePlayerResultEvent;

        public event PlayFabRequestEvent<DeletePlayerSharedSecretRequest> OnAdminDeletePlayerSharedSecretRequestEvent;

        public event PlayFabResultEvent<DeletePlayerSharedSecretResult> OnAdminDeletePlayerSharedSecretResultEvent;

        public event PlayFabRequestEvent<DeleteStoreRequest> OnAdminDeleteStoreRequestEvent;

        public event PlayFabResultEvent<DeleteStoreResult> OnAdminDeleteStoreResultEvent;

        public event PlayFabRequestEvent<DeleteTaskRequest> OnAdminDeleteTaskRequestEvent;

        public event PlayFabResultEvent<EmptyResponse> OnAdminDeleteTaskResultEvent;

        public event PlayFabRequestEvent<DeleteTitleRequest> OnAdminDeleteTitleRequestEvent;

        public event PlayFabResultEvent<DeleteTitleResult> OnAdminDeleteTitleResultEvent;

        public event PlayFabRequestEvent<DeleteTitleDataOverrideRequest> OnAdminDeleteTitleDataOverrideRequestEvent;

        public event PlayFabResultEvent<DeleteTitleDataOverrideResult> OnAdminDeleteTitleDataOverrideResultEvent;

        public event PlayFabRequestEvent<ExportMasterPlayerDataRequest> OnAdminExportMasterPlayerDataRequestEvent;

        public event PlayFabResultEvent<ExportMasterPlayerDataResult> OnAdminExportMasterPlayerDataResultEvent;

        public event PlayFabRequestEvent<GetTaskInstanceRequest> OnAdminGetActionsOnPlayersInSegmentTaskInstanceRequestEvent;

        public event PlayFabResultEvent<GetActionsOnPlayersInSegmentTaskInstanceResult> OnAdminGetActionsOnPlayersInSegmentTaskInstanceResultEvent;

        public event PlayFabRequestEvent<GetAllSegmentsRequest> OnAdminGetAllSegmentsRequestEvent;

        public event PlayFabResultEvent<GetAllSegmentsResult> OnAdminGetAllSegmentsResultEvent;

        public event PlayFabRequestEvent<GetCatalogItemsRequest> OnAdminGetCatalogItemsRequestEvent;

        public event PlayFabResultEvent<GetCatalogItemsResult> OnAdminGetCatalogItemsResultEvent;

        public event PlayFabRequestEvent<GetCloudScriptRevisionRequest> OnAdminGetCloudScriptRevisionRequestEvent;

        public event PlayFabResultEvent<GetCloudScriptRevisionResult> OnAdminGetCloudScriptRevisionResultEvent;

        public event PlayFabRequestEvent<GetTaskInstanceRequest> OnAdminGetCloudScriptTaskInstanceRequestEvent;

        public event PlayFabResultEvent<GetCloudScriptTaskInstanceResult> OnAdminGetCloudScriptTaskInstanceResultEvent;

        public event PlayFabRequestEvent<GetCloudScriptVersionsRequest> OnAdminGetCloudScriptVersionsRequestEvent;

        public event PlayFabResultEvent<GetCloudScriptVersionsResult> OnAdminGetCloudScriptVersionsResultEvent;

        public event PlayFabRequestEvent<GetContentListRequest> OnAdminGetContentListRequestEvent;

        public event PlayFabResultEvent<GetContentListResult> OnAdminGetContentListResultEvent;

        public event PlayFabRequestEvent<GetContentUploadUrlRequest> OnAdminGetContentUploadUrlRequestEvent;

        public event PlayFabResultEvent<GetContentUploadUrlResult> OnAdminGetContentUploadUrlResultEvent;

        public event PlayFabRequestEvent<GetDataReportRequest> OnAdminGetDataReportRequestEvent;

        public event PlayFabResultEvent<GetDataReportResult> OnAdminGetDataReportResultEvent;

        public event PlayFabRequestEvent<GetMatchmakerGameInfoRequest> OnAdminGetMatchmakerGameInfoRequestEvent;

        public event PlayFabResultEvent<GetMatchmakerGameInfoResult> OnAdminGetMatchmakerGameInfoResultEvent;

        public event PlayFabRequestEvent<GetMatchmakerGameModesRequest> OnAdminGetMatchmakerGameModesRequestEvent;

        public event PlayFabResultEvent<GetMatchmakerGameModesResult> OnAdminGetMatchmakerGameModesResultEvent;

        public event PlayFabRequestEvent<GetPlayedTitleListRequest> OnAdminGetPlayedTitleListRequestEvent;

        public event PlayFabResultEvent<GetPlayedTitleListResult> OnAdminGetPlayedTitleListResultEvent;

        public event PlayFabRequestEvent<GetPlayerIdFromAuthTokenRequest> OnAdminGetPlayerIdFromAuthTokenRequestEvent;

        public event PlayFabResultEvent<GetPlayerIdFromAuthTokenResult> OnAdminGetPlayerIdFromAuthTokenResultEvent;

        public event PlayFabRequestEvent<GetPlayerProfileRequest> OnAdminGetPlayerProfileRequestEvent;

        public event PlayFabResultEvent<GetPlayerProfileResult> OnAdminGetPlayerProfileResultEvent;

        public event PlayFabRequestEvent<GetPlayersSegmentsRequest> OnAdminGetPlayerSegmentsRequestEvent;

        public event PlayFabResultEvent<GetPlayerSegmentsResult> OnAdminGetPlayerSegmentsResultEvent;

        public event PlayFabRequestEvent<GetPlayerSharedSecretsRequest> OnAdminGetPlayerSharedSecretsRequestEvent;

        public event PlayFabResultEvent<GetPlayerSharedSecretsResult> OnAdminGetPlayerSharedSecretsResultEvent;

        public event PlayFabRequestEvent<GetPlayersInSegmentRequest> OnAdminGetPlayersInSegmentRequestEvent;

        public event PlayFabResultEvent<GetPlayersInSegmentResult> OnAdminGetPlayersInSegmentResultEvent;

        public event PlayFabRequestEvent<GetPlayerStatisticDefinitionsRequest> OnAdminGetPlayerStatisticDefinitionsRequestEvent;

        public event PlayFabResultEvent<GetPlayerStatisticDefinitionsResult> OnAdminGetPlayerStatisticDefinitionsResultEvent;

        public event PlayFabRequestEvent<GetPlayerStatisticVersionsRequest> OnAdminGetPlayerStatisticVersionsRequestEvent;

        public event PlayFabResultEvent<GetPlayerStatisticVersionsResult> OnAdminGetPlayerStatisticVersionsResultEvent;

        public event PlayFabRequestEvent<GetPlayerTagsRequest> OnAdminGetPlayerTagsRequestEvent;

        public event PlayFabResultEvent<GetPlayerTagsResult> OnAdminGetPlayerTagsResultEvent;

        public event PlayFabRequestEvent<GetPolicyRequest> OnAdminGetPolicyRequestEvent;

        public event PlayFabResultEvent<GetPolicyResponse> OnAdminGetPolicyResultEvent;

        public event PlayFabRequestEvent<GetPublisherDataRequest> OnAdminGetPublisherDataRequestEvent;

        public event PlayFabResultEvent<GetPublisherDataResult> OnAdminGetPublisherDataResultEvent;

        public event PlayFabRequestEvent<GetRandomResultTablesRequest> OnAdminGetRandomResultTablesRequestEvent;

        public event PlayFabResultEvent<GetRandomResultTablesResult> OnAdminGetRandomResultTablesResultEvent;

        public event PlayFabRequestEvent<GetServerBuildInfoRequest> OnAdminGetServerBuildInfoRequestEvent;

        public event PlayFabResultEvent<GetServerBuildInfoResult> OnAdminGetServerBuildInfoResultEvent;

        public event PlayFabRequestEvent<GetServerBuildUploadURLRequest> OnAdminGetServerBuildUploadUrlRequestEvent;

        public event PlayFabResultEvent<GetServerBuildUploadURLResult> OnAdminGetServerBuildUploadUrlResultEvent;

        public event PlayFabRequestEvent<GetStoreItemsRequest> OnAdminGetStoreItemsRequestEvent;

        public event PlayFabResultEvent<GetStoreItemsResult> OnAdminGetStoreItemsResultEvent;

        public event PlayFabRequestEvent<GetTaskInstancesRequest> OnAdminGetTaskInstancesRequestEvent;

        public event PlayFabResultEvent<GetTaskInstancesResult> OnAdminGetTaskInstancesResultEvent;

        public event PlayFabRequestEvent<GetTasksRequest> OnAdminGetTasksRequestEvent;

        public event PlayFabResultEvent<GetTasksResult> OnAdminGetTasksResultEvent;

        public event PlayFabRequestEvent<GetTitleDataRequest> OnAdminGetTitleDataRequestEvent;

        public event PlayFabResultEvent<GetTitleDataResult> OnAdminGetTitleDataResultEvent;

        public event PlayFabRequestEvent<GetTitleDataRequest> OnAdminGetTitleInternalDataRequestEvent;

        public event PlayFabResultEvent<GetTitleDataResult> OnAdminGetTitleInternalDataResultEvent;

        public event PlayFabRequestEvent<LookupUserAccountInfoRequest> OnAdminGetUserAccountInfoRequestEvent;

        public event PlayFabResultEvent<LookupUserAccountInfoResult> OnAdminGetUserAccountInfoResultEvent;

        public event PlayFabRequestEvent<GetUserBansRequest> OnAdminGetUserBansRequestEvent;

        public event PlayFabResultEvent<GetUserBansResult> OnAdminGetUserBansResultEvent;

        public event PlayFabRequestEvent<GetUserDataRequest> OnAdminGetUserDataRequestEvent;

        public event PlayFabResultEvent<GetUserDataResult> OnAdminGetUserDataResultEvent;

        public event PlayFabRequestEvent<GetUserDataRequest> OnAdminGetUserInternalDataRequestEvent;

        public event PlayFabResultEvent<GetUserDataResult> OnAdminGetUserInternalDataResultEvent;

        public event PlayFabRequestEvent<GetUserInventoryRequest> OnAdminGetUserInventoryRequestEvent;

        public event PlayFabResultEvent<GetUserInventoryResult> OnAdminGetUserInventoryResultEvent;

        public event PlayFabRequestEvent<GetUserDataRequest> OnAdminGetUserPublisherDataRequestEvent;

        public event PlayFabResultEvent<GetUserDataResult> OnAdminGetUserPublisherDataResultEvent;

        public event PlayFabRequestEvent<GetUserDataRequest> OnAdminGetUserPublisherInternalDataRequestEvent;

        public event PlayFabResultEvent<GetUserDataResult> OnAdminGetUserPublisherInternalDataResultEvent;

        public event PlayFabRequestEvent<GetUserDataRequest> OnAdminGetUserPublisherReadOnlyDataRequestEvent;

        public event PlayFabResultEvent<GetUserDataResult> OnAdminGetUserPublisherReadOnlyDataResultEvent;

        public event PlayFabRequestEvent<GetUserDataRequest> OnAdminGetUserReadOnlyDataRequestEvent;

        public event PlayFabResultEvent<GetUserDataResult> OnAdminGetUserReadOnlyDataResultEvent;

        public event PlayFabRequestEvent<GrantItemsToUsersRequest> OnAdminGrantItemsToUsersRequestEvent;

        public event PlayFabResultEvent<GrantItemsToUsersResult> OnAdminGrantItemsToUsersResultEvent;

        public event PlayFabRequestEvent<IncrementLimitedEditionItemAvailabilityRequest> OnAdminIncrementLimitedEditionItemAvailabilityRequestEvent;

        public event PlayFabResultEvent<IncrementLimitedEditionItemAvailabilityResult> OnAdminIncrementLimitedEditionItemAvailabilityResultEvent;

        public event PlayFabRequestEvent<IncrementPlayerStatisticVersionRequest> OnAdminIncrementPlayerStatisticVersionRequestEvent;

        public event PlayFabResultEvent<IncrementPlayerStatisticVersionResult> OnAdminIncrementPlayerStatisticVersionResultEvent;

        public event PlayFabRequestEvent<ListOpenIdConnectionRequest> OnAdminListOpenIdConnectionRequestEvent;

        public event PlayFabResultEvent<ListOpenIdConnectionResponse> OnAdminListOpenIdConnectionResultEvent;

        public event PlayFabRequestEvent<ListBuildsRequest> OnAdminListServerBuildsRequestEvent;

        public event PlayFabResultEvent<ListBuildsResult> OnAdminListServerBuildsResultEvent;

        public event PlayFabRequestEvent<ListVirtualCurrencyTypesRequest> OnAdminListVirtualCurrencyTypesRequestEvent;

        public event PlayFabResultEvent<ListVirtualCurrencyTypesResult> OnAdminListVirtualCurrencyTypesResultEvent;

        public event PlayFabRequestEvent<ModifyMatchmakerGameModesRequest> OnAdminModifyMatchmakerGameModesRequestEvent;

        public event PlayFabResultEvent<ModifyMatchmakerGameModesResult> OnAdminModifyMatchmakerGameModesResultEvent;

        public event PlayFabRequestEvent<ModifyServerBuildRequest> OnAdminModifyServerBuildRequestEvent;

        public event PlayFabResultEvent<ModifyServerBuildResult> OnAdminModifyServerBuildResultEvent;

        public event PlayFabRequestEvent<RefundPurchaseRequest> OnAdminRefundPurchaseRequestEvent;

        public event PlayFabResultEvent<RefundPurchaseResponse> OnAdminRefundPurchaseResultEvent;

        public event PlayFabRequestEvent<RemovePlayerTagRequest> OnAdminRemovePlayerTagRequestEvent;

        public event PlayFabResultEvent<RemovePlayerTagResult> OnAdminRemovePlayerTagResultEvent;

        public event PlayFabRequestEvent<RemoveServerBuildRequest> OnAdminRemoveServerBuildRequestEvent;

        public event PlayFabResultEvent<RemoveServerBuildResult> OnAdminRemoveServerBuildResultEvent;

        public event PlayFabRequestEvent<RemoveVirtualCurrencyTypesRequest> OnAdminRemoveVirtualCurrencyTypesRequestEvent;

        public event PlayFabResultEvent<BlankResult> OnAdminRemoveVirtualCurrencyTypesResultEvent;

        public event PlayFabRequestEvent<ResetCharacterStatisticsRequest> OnAdminResetCharacterStatisticsRequestEvent;

        public event PlayFabResultEvent<ResetCharacterStatisticsResult> OnAdminResetCharacterStatisticsResultEvent;

        public event PlayFabRequestEvent<ResetPasswordRequest> OnAdminResetPasswordRequestEvent;

        public event PlayFabResultEvent<ResetPasswordResult> OnAdminResetPasswordResultEvent;

        public event PlayFabRequestEvent<ResetUserStatisticsRequest> OnAdminResetUserStatisticsRequestEvent;

        public event PlayFabResultEvent<ResetUserStatisticsResult> OnAdminResetUserStatisticsResultEvent;

        public event PlayFabRequestEvent<ResolvePurchaseDisputeRequest> OnAdminResolvePurchaseDisputeRequestEvent;

        public event PlayFabResultEvent<ResolvePurchaseDisputeResponse> OnAdminResolvePurchaseDisputeResultEvent;

        public event PlayFabRequestEvent<RevokeAllBansForUserRequest> OnAdminRevokeAllBansForUserRequestEvent;

        public event PlayFabResultEvent<RevokeAllBansForUserResult> OnAdminRevokeAllBansForUserResultEvent;

        public event PlayFabRequestEvent<RevokeBansRequest> OnAdminRevokeBansRequestEvent;

        public event PlayFabResultEvent<RevokeBansResult> OnAdminRevokeBansResultEvent;

        public event PlayFabRequestEvent<RevokeInventoryItemRequest> OnAdminRevokeInventoryItemRequestEvent;

        public event PlayFabResultEvent<RevokeInventoryResult> OnAdminRevokeInventoryItemResultEvent;

        public event PlayFabRequestEvent<RevokeInventoryItemsRequest> OnAdminRevokeInventoryItemsRequestEvent;

        public event PlayFabResultEvent<RevokeInventoryItemsResult> OnAdminRevokeInventoryItemsResultEvent;

        public event PlayFabRequestEvent<RunTaskRequest> OnAdminRunTaskRequestEvent;

        public event PlayFabResultEvent<RunTaskResult> OnAdminRunTaskResultEvent;

        public event PlayFabRequestEvent<SendAccountRecoveryEmailRequest> OnAdminSendAccountRecoveryEmailRequestEvent;

        public event PlayFabResultEvent<SendAccountRecoveryEmailResult> OnAdminSendAccountRecoveryEmailResultEvent;

        public event PlayFabRequestEvent<UpdateCatalogItemsRequest> OnAdminSetCatalogItemsRequestEvent;

        public event PlayFabResultEvent<UpdateCatalogItemsResult> OnAdminSetCatalogItemsResultEvent;

        public event PlayFabRequestEvent<SetPlayerSecretRequest> OnAdminSetPlayerSecretRequestEvent;

        public event PlayFabResultEvent<SetPlayerSecretResult> OnAdminSetPlayerSecretResultEvent;

        public event PlayFabRequestEvent<SetPublishedRevisionRequest> OnAdminSetPublishedRevisionRequestEvent;

        public event PlayFabResultEvent<SetPublishedRevisionResult> OnAdminSetPublishedRevisionResultEvent;

        public event PlayFabRequestEvent<SetPublisherDataRequest> OnAdminSetPublisherDataRequestEvent;

        public event PlayFabResultEvent<SetPublisherDataResult> OnAdminSetPublisherDataResultEvent;

        public event PlayFabRequestEvent<UpdateStoreItemsRequest> OnAdminSetStoreItemsRequestEvent;

        public event PlayFabResultEvent<UpdateStoreItemsResult> OnAdminSetStoreItemsResultEvent;

        public event PlayFabRequestEvent<SetTitleDataRequest> OnAdminSetTitleDataRequestEvent;

        public event PlayFabResultEvent<SetTitleDataResult> OnAdminSetTitleDataResultEvent;

        public event PlayFabRequestEvent<SetTitleDataAndOverridesRequest> OnAdminSetTitleDataAndOverridesRequestEvent;

        public event PlayFabResultEvent<SetTitleDataAndOverridesResult> OnAdminSetTitleDataAndOverridesResultEvent;

        public event PlayFabRequestEvent<SetTitleDataRequest> OnAdminSetTitleInternalDataRequestEvent;

        public event PlayFabResultEvent<SetTitleDataResult> OnAdminSetTitleInternalDataResultEvent;

        public event PlayFabRequestEvent<SetupPushNotificationRequest> OnAdminSetupPushNotificationRequestEvent;

        public event PlayFabResultEvent<SetupPushNotificationResult> OnAdminSetupPushNotificationResultEvent;

        public event PlayFabRequestEvent<SubtractUserVirtualCurrencyRequest> OnAdminSubtractUserVirtualCurrencyRequestEvent;

        public event PlayFabResultEvent<ModifyUserVirtualCurrencyResult> OnAdminSubtractUserVirtualCurrencyResultEvent;

        public event PlayFabRequestEvent<UpdateBansRequest> OnAdminUpdateBansRequestEvent;

        public event PlayFabResultEvent<UpdateBansResult> OnAdminUpdateBansResultEvent;

        public event PlayFabRequestEvent<UpdateCatalogItemsRequest> OnAdminUpdateCatalogItemsRequestEvent;

        public event PlayFabResultEvent<UpdateCatalogItemsResult> OnAdminUpdateCatalogItemsResultEvent;

        public event PlayFabRequestEvent<UpdateCloudScriptRequest> OnAdminUpdateCloudScriptRequestEvent;

        public event PlayFabResultEvent<UpdateCloudScriptResult> OnAdminUpdateCloudScriptResultEvent;

        public event PlayFabRequestEvent<UpdateOpenIdConnectionRequest> OnAdminUpdateOpenIdConnectionRequestEvent;

        public event PlayFabResultEvent<EmptyResponse> OnAdminUpdateOpenIdConnectionResultEvent;

        public event PlayFabRequestEvent<UpdatePlayerSharedSecretRequest> OnAdminUpdatePlayerSharedSecretRequestEvent;

        public event PlayFabResultEvent<UpdatePlayerSharedSecretResult> OnAdminUpdatePlayerSharedSecretResultEvent;

        public event PlayFabRequestEvent<UpdatePlayerStatisticDefinitionRequest> OnAdminUpdatePlayerStatisticDefinitionRequestEvent;

        public event PlayFabResultEvent<UpdatePlayerStatisticDefinitionResult> OnAdminUpdatePlayerStatisticDefinitionResultEvent;

        public event PlayFabRequestEvent<UpdatePolicyRequest> OnAdminUpdatePolicyRequestEvent;

        public event PlayFabResultEvent<UpdatePolicyResponse> OnAdminUpdatePolicyResultEvent;

        public event PlayFabRequestEvent<UpdateRandomResultTablesRequest> OnAdminUpdateRandomResultTablesRequestEvent;

        public event PlayFabResultEvent<UpdateRandomResultTablesResult> OnAdminUpdateRandomResultTablesResultEvent;

        public event PlayFabRequestEvent<UpdateStoreItemsRequest> OnAdminUpdateStoreItemsRequestEvent;

        public event PlayFabResultEvent<UpdateStoreItemsResult> OnAdminUpdateStoreItemsResultEvent;

        public event PlayFabRequestEvent<UpdateTaskRequest> OnAdminUpdateTaskRequestEvent;

        public event PlayFabResultEvent<EmptyResponse> OnAdminUpdateTaskResultEvent;

        public event PlayFabRequestEvent<UpdateUserDataRequest> OnAdminUpdateUserDataRequestEvent;

        public event PlayFabResultEvent<UpdateUserDataResult> OnAdminUpdateUserDataResultEvent;

        public event PlayFabRequestEvent<UpdateUserInternalDataRequest> OnAdminUpdateUserInternalDataRequestEvent;

        public event PlayFabResultEvent<UpdateUserDataResult> OnAdminUpdateUserInternalDataResultEvent;

        public event PlayFabRequestEvent<UpdateUserDataRequest> OnAdminUpdateUserPublisherDataRequestEvent;

        public event PlayFabResultEvent<UpdateUserDataResult> OnAdminUpdateUserPublisherDataResultEvent;

        public event PlayFabRequestEvent<UpdateUserInternalDataRequest> OnAdminUpdateUserPublisherInternalDataRequestEvent;

        public event PlayFabResultEvent<UpdateUserDataResult> OnAdminUpdateUserPublisherInternalDataResultEvent;

        public event PlayFabRequestEvent<UpdateUserDataRequest> OnAdminUpdateUserPublisherReadOnlyDataRequestEvent;

        public event PlayFabResultEvent<UpdateUserDataResult> OnAdminUpdateUserPublisherReadOnlyDataResultEvent;

        public event PlayFabRequestEvent<UpdateUserDataRequest> OnAdminUpdateUserReadOnlyDataRequestEvent;

        public event PlayFabResultEvent<UpdateUserDataResult> OnAdminUpdateUserReadOnlyDataResultEvent;

        public event PlayFabRequestEvent<UpdateUserTitleDisplayNameRequest> OnAdminUpdateUserTitleDisplayNameRequestEvent;

        public event PlayFabResultEvent<UpdateUserTitleDisplayNameResult> OnAdminUpdateUserTitleDisplayNameResultEvent;
    }
}

//#endif