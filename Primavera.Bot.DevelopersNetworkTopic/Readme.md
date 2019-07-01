Topic Creation instructions

------------------------------------------------------------------------------------------------------------------------
      
# To create a new Topic Handler execute the following steps

1. Read the "TopicStyleGuide.md" document first.
2. Give the topic a name. Replace all ocurrences of "TemplateTopic" from this project with the name you chose.
3. Check the guids class and generate a new Guid.
4. Put the project into the Topics Main SLN for compilation purposes.
5. Define the tests you wish to produce. Move the TestClass to the tests project and implement all required signatures/classes.
6. Remove the "Microsoft.VisualStudio.QualityTools.UnitTestFramework" reference from this project.
7. Review the topic.config and decide what your topic will be after reading its instructions. Configure the tasks, and the integration config with the pipelines/handlers required.
8. Define the handlers you need to produce and implement them. View the template for a simple create messages type handler.
9. Implement the tests for the handlers. Test them isolated and/or as part of a pipeline. That's a developer decision.
10. Enable Code Analysis and run StyleCop on the final code.
11. Request the deployment team to deploy your new topic on the bot module.
12. The SNK key must be changed to the one used in the company.
